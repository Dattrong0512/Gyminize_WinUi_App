 DROP TABLE IF EXISTS Payment;
 DROP TABLE IF EXISTS OrderDetail;
 DROP TABLE IF EXISTS Orders;
 DROP TABLE IF EXISTS Product;
 DROP TABLE IF EXISTS Category;
 DROP TABLE IF EXISTS FoodDetail;
 DROP TABLE IF EXISTS Food;
 DROP TABLE IF EXISTS DailyDiary;
 DROP TABLE IF EXISTS ExerciseDetail;
 DROP TABLE IF EXISTS Exercise;
 DROP TABLE IF EXISTS WorkoutDetail;
 DROP TABLE IF EXISTS Typeworkout;
 DROP TABLE IF EXISTS PlanDetail;
 DROP TABLE IF EXISTS Plan;
 DROP TABLE IF EXISTS Customer_Health;
 DROP TABLE IF EXISTS Customer;


 
CREATE TABLE IF NOT EXISTS Customer (
    customer_id SERIAL PRIMARY KEY,
    customer_name VARCHAR(100),
    auth_type INT NOT NULL, 
    username VARCHAR(100), -- thông username/password (có thể null nếu dùng email)         
    customer_password VARCHAR(255),               
    role_user INT,
	email VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Customer_Health (
    customer_id INT PRIMARY KEY, 
    gender INT CHECK (gender IN (0, 1)),
    height INT,                                          
    weight INT,                                          
    age INT,                                             
    activity_level DECIMAL,                                  
    body_fat DECIMAL(10, 2),                              
    tdee DECIMAL(10, 2),                                  
    FOREIGN KEY (customer_id) REFERENCES Customer(customer_id) ON DELETE CASCADE
);


-- Tạo bảng DailyDiary
CREATE TABLE IF NOT EXISTS DailyDiary (
    dailydiary_id SERIAL PRIMARY KEY,
    customer_id INT REFERENCES Customer(customer_id),
    diary_date DATE NOT NULL,
    calories_remain DECIMAL(10,2) NOT NULL,
	daily_weight INT NOT NULL,
	total_calories DECIMAL(10,2) NOT NULL,
    notes TEXT
);

-- Tạo bảng Food với cột serving_unit mới
CREATE TABLE IF NOT EXISTS Food (
    food_id SERIAL PRIMARY KEY,
    food_name VARCHAR(255) NOT NULL,
    calories INT,  -- lượng calo tính trên mỗi khẩu phần ăn
    protein FLOAT,
    carbs FLOAT,
    fats FLOAT,
    serving_unit VARCHAR(50) -- đơn vị khẩu phần ăn (ví dụ: 1 tô, 200g)
);

-- Tạo bảng FoodDetail
CREATE TABLE IF NOT EXISTS FoodDetail (
    fooddetail_id SERIAL PRIMARY KEY,
    dailydiary_id INT REFERENCES DailyDiary(dailydiary_id),
    food_id INT REFERENCES Food(food_id) ON DELETE CASCADE, -- NULL nếu món ăn riêng
    meal_type INT CHECK (meal_type IN (1, 2, 3, 4)), -- Theo thứ tự (Sáng, Trưa, Chiều, Tối)
    food_amount INT -- kích cỡ khẩu phần ăn (gram)
);

-- Tạo bảng Category
CREATE TABLE IF NOT EXISTS Category (
    category_id SERIAL PRIMARY KEY,
    category_name VARCHAR(255) NOT NULL,
    description TEXT
);

-- Tạo bảng Product
CREATE TABLE IF NOT EXISTS Product (
    product_id SERIAL PRIMARY KEY,
    category_id INT REFERENCES Category(category_id),
    product_name VARCHAR(255) NOT NULL,
    product_price DECIMAL(10, 2),
    product_provider VARCHAR(255),
    description TEXT
);

-- Tạo bảng Orders
CREATE TABLE IF NOT EXISTS Orders (
    orders_id SERIAL PRIMARY KEY,
    customer_id INT REFERENCES Customer(customer_id),
    order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	address TEXT,
	phone_number VARCHAR(10),
    total_price DECIMAL(10, 2)
);

CREATE TABLE IF NOT EXISTS Payment (
    payment_id SERIAL PRIMARY KEY,                       -- Khóa chính tự tăng
    orders_id INT REFERENCES Orders(orders_id) ON DELETE CASCADE, -- Liên kết với bảng Orders
    payment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,    -- Thời gian thanh toán
    payment_method VARCHAR(50) NOT NULL,                -- Phương thức thanh toán (VD: Cash, Credit Card, Paypal)
    payment_status VARCHAR(50) CHECK (payment_status IN ('Pending', 'Completed', 'Failed', 'Refunded')), -- Trạng thái thanh toán
    payment_amount DECIMAL(10, 2) NOT NULL,             -- Số tiền thanh toán
    transaction_id VARCHAR(100),                        -- Mã giao dịch (nếu có, dùng cho các cổng thanh toán)
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP      -- Thời gian tạo bản ghi
);
-- Tạo bảng OrderDetail
CREATE TABLE IF NOT EXISTS OrderDetail (
    orderdetail_id SERIAL PRIMARY KEY,
    product_id INT REFERENCES Product(product_id),
    orders_id INT REFERENCES Orders(orders_id),
    product_amount INT NOT NULL
);



-- Tạo bảng Plan
CREATE TABLE IF NOT EXISTS Plan (
    plan_id SERIAL PRIMARY KEY,
    plan_name VARCHAR(255) NOT NULL,
    description TEXT,
    duration_week INT NOT NULL --thời lượng kế hoạch theo tuần
);

-- Tạo bảng PlanDetail
CREATE TABLE IF NOT EXISTS PlanDetail (
    plandetail_id SERIAL PRIMARY KEY,
    plan_id INT REFERENCES Plan(plan_id),
    customer_id INT REFERENCES Customer(customer_id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    start_date DATE,
    end_date DATE
);

-- Tạo bảng WorkoutDay
CREATE TABLE IF NOT EXISTS Typeworkout (
    typeworkout_id SERIAL PRIMARY KEY,
    workoutday_type VARCHAR(100), --push/pull/leg
    description TEXT
);

CREATE TABLE IF NOT EXISTS WorkoutDetail (
    workoutdetail_id SERIAL PRIMARY KEY,
    typeworkout_id INT REFERENCES Typeworkout(typeworkout_id),
    plandetail_id INT REFERENCES PlanDetail(plandetail_id), -- Update foreign key to reference PlanDetail
    date_workout DATE,
    description TEXT
);

-- Tạo bảng Exercise
CREATE TABLE IF NOT EXISTS Exercise (
    exercise_id SERIAL PRIMARY KEY,
    exercise_name VARCHAR(255) NOT NULL,
    description text,
    linkvideo text,
    reps INT
);

-- Tạo bảng ExerciseDetail
CREATE TABLE IF NOT EXISTS ExerciseDetail (
    exercisedetail_id SERIAL PRIMARY KEY,
    typeworkout_id INT REFERENCES Typeworkout(typeworkout_id),
    exercise_id INT REFERENCES Exercise(exercise_id),
    workout_sets INT
);


-- Thiết lập lại chuỗi giá trị của các cột SERIAL
ALTER SEQUENCE customer_customer_id_seq RESTART WITH 1;
ALTER SEQUENCE plan_plan_id_seq RESTART WITH 1;
ALTER SEQUENCE plandetail_plandetail_id_seq RESTART WITH 1;
ALTER sequence Typeworkout_typeworkout_id_seq RESTART WITH 1;
ALTER SEQUENCE workoutdetail_workoutdetail_id_seq RESTART WITH 1;
ALTER SEQUENCE exercise_exercise_id_seq RESTART WITH 1;
ALTER SEQUENCE exercisedetail_exercisedetail_id_seq RESTART WITH 1;
ALTER SEQUENCE dailydiary_dailydiary_id_seq RESTART WITH 1;
ALTER SEQUENCE food_food_id_seq RESTART WITH 1;
ALTER SEQUENCE fooddetail_fooddetail_id_seq RESTART WITH 1;
ALTER SEQUENCE category_category_id_seq RESTART WITH 1;
ALTER SEQUENCE product_product_id_seq RESTART WITH 1;
ALTER SEQUENCE orderdetail_orderdetail_id_seq RESTART WITH 1;
ALTER SEQUENCE orders_orders_id_seq RESTART WITH 1;
ALTER SEQUENCE payment_payment_id_seq RESTART WITH 1;


 -- Chèn dữ liệu vào bảng Customer
INSERT INTO Customer (customer_name, auth_type, username, customer_password, role_user, email)
VALUES 
('John Doe', 1, 'johndoe', 'password123', 2,'gyminize123@gmail.com'),
('Jane Smith', 1, 'janesmith', 'password456', 1,'gyminize123@gmail.com'),
('Alice Brown', 1, 'alicebrown', 'password789', 2,'gyminize123@gmail.com'),
('Bob Green', 1, 'bobgreen', 'password321', 1,'gyminize123@gmail.com'),
('Charlie Blue', 1, 'charlieblue', 'password654', 2,'gyminize123@gmail.com'),
('David Black', 1, 'davidblack', 'password987', 1,'gyminize123@gmail.com'),
('Emma White', 1, 'emmawhite', 'password111', 2,'gyminize123@gmail.com'),
('Fiona Red', 1, 'fionared', 'password222', 1,'gyminize123@gmail.com'),
('George Yellow', 1, 'georgeyellow', 'password333', 2,'gyminize123@gmail.com'),
('Helen Purple', 1, 'helenpurple', 'password444', 1,'gyminize123@gmail.com');

-- Chèn dữ liệu vào bảng Customer_Health
INSERT INTO Customer_Health (customer_id, gender, height, weight, age, activity_level, body_fat, tdee)
VALUES 
(1, 1, 180, 75, 28, 1.2, 20.5, 2200),
(2, 0, 165, 60, 25, 1.5, 18.0, 1800),
(3, 0, 170, 65, 30, 1.3, 19.5, 1900),
(4, 1, 175, 80, 35, 1.4, 22.0, 2500),
(5, 0, 160, 50, 29, 1.6, 15.0, 1700),
(6, 1, 185, 90, 32, 1.7, 24.5, 2800),
(7, 0, 155, 45, 23, 1.8, 14.5, 1500),
(8, 1, 190, 95, 40, 1.5, 26.0, 3000),
(9, 0, 168, 58, 26, 1.2, 17.0, 1750),
(10, 1, 178, 70, 33, 1.6, 21.0, 2300);

-- Chèn dữ liệu vào bảng DailyDiary
INSERT INTO DailyDiary (customer_id, diary_date, calories_remain, daily_weight, total_calories, notes)
VALUES 
(1, '2024-11-01', 500, 75, 2200, 'Feeling great today'),
(2, '2024-11-01', 200, 60, 1800, 'Busy day'),
(3, '2024-11-01', 300, 65, 1900, 'Moderate workout'),
(4, '2024-11-02', 100, 80, 2500, 'Heavy workout'),
(5, '2024-11-02', 250, 50, 1700, 'Light meals today'),
(6, '2024-11-03', 400, 90, 2800, 'Good training'),
(7, '2024-11-03', 350, 45, 1500, 'Feeling okay'),
(8, '2024-11-04', 600, 95, 3000, 'Intense gym session'),
(9, '2024-11-04', 150, 58, 1750, 'Moderate day'),
(10, '2024-11-04', 500, 70, 2300, 'Relaxed day');

-- Chèn dữ liệu vào bảng Food
INSERT INTO Food (food_name, calories, protein, carbs, fats, serving_unit)
VALUES 
('Chicken Breast', 165, 31.0, 0.0, 3.6, '100g'),
('Rice', 130, 2.7, 28.0, 0.3, '100g'),
('Broccoli', 55, 3.7, 11.0, 0.6, '100g'),
('Apple', 52, 0.3, 14.0, 0.2, '1 quả'),
('Salmon', 208, 20.0, 0.0, 13.0, '100g'),
('Eggs', 155, 13.0, 1.1, 11.0, '1 quả'),
('Oatmeal', 389, 16.9, 66.3, 6.9, '100g'),
('Almonds', 579, 21.2, 21.6, 49.4, '100g'),
('Milk', 42, 3.4, 4.8, 1.0, '100ml'),
('Pasta', 131, 5.0, 25.0, 1.1, '100g');

-- Chèn dữ liệu vào bảng FoodDetail
INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount)
VALUES 
(1, 1, 1, 200),
(2, 2, 2, 300),
(3, 3, 3, 100),
(4, 4, 4, 1),
(5, 5, 1, 150),
(6, 6, 2, 200),
(7, 7, 3, 50),
(8, 8, 4, 30),
(9, 9, 1, 250),
(10, 10, 2, 100);

-- Chèn dữ liệu vào bảng Category
INSERT INTO Category (category_name, description)
VALUES 
('Supplements', 'Dinh dưỡng bổ sung'),
('Equipment', 'Thiết bị tập luyện'),
('Apparel', 'Trang phục tập gym'),
('Accessories', 'Phụ kiện tập luyện'),
('Food', 'Thực phẩm dinh dưỡng'),
('Beverages', 'Đồ uống thể thao'),
('Training Gear', 'Dụng cụ tập luyện'),
('Fitness Devices', 'Thiết bị theo dõi sức khỏe'),
('Gym Membership', 'Thẻ thành viên phòng gym'),
('Personal Training', 'Dịch vụ huấn luyện viên cá nhân');

-- Chèn dữ liệu vào bảng Product
INSERT INTO Product (category_id, product_name, product_price, product_provider, description)
VALUES 
(1, 'Whey Protein', 50.00, 'NutriPro', 'Bột protein chất lượng cao'),
(2, 'Yoga Mat', 20.00, 'FitGear', 'Thảm tập yoga chống trượt'),
(3, 'Dumbbells', 30.00, 'GymTools', 'Tạ tay 10kg'),
(4, 'Resistance Bands', 15.00, 'FlexBand', 'Bộ dây kháng lực tập luyện'),
(5, 'Creatine', 25.00, 'PowerLab', 'Bổ sung creatine'),
(6, 'T-shirt', 10.00, 'FitWear', 'Áo thun thể thao'),
(7, 'Water Bottle', 8.00, 'HydratePlus', 'Bình nước giữ nhiệt'),
(8, 'Fitness Tracker', 100.00, 'SmartHealth', 'Thiết bị theo dõi sức khỏe'),
(9, 'Pre-workout', 35.00, 'EnergyMax', 'Thực phẩm bổ sung năng lượng trước tập luyện'),
(10, 'Gym Membership 1 Month', 40.00, 'FitnessHub', 'Thẻ tập gym 1 tháng');

-- Chèn dữ liệu vào bảng Orders
INSERT INTO Orders (customer_id, total_price,address,phone_number)
VALUES 
(1, 50.00,'123 Hai ba trung, Ha noi','0123456789'),
(2, 20.00,'123 Hai ba trung, Ha noi','0123456789'),
(3, 30.00,'123 Hai ba trung, Ha noi','0123456789'),
(4, 15.00,'123 Hai ba trung, Ha noi','0123456789'),
(5, 25.00,'123 Hai ba trung, Ha noi','0123456789'),
(6, 10.00,'123 Hai ba trung, Ha noi','0123456789'),
(7, 8.00,'123 Hai ba trung, Ha noi','0123456789'),
(8, 100.00,'123 Hai ba trung, Ha noi','0123456789'),
(9, 35.00,'123 Hai ba trung, Ha noi','0123456789'),
(10, 40.00,'123 Hai ba trung, Ha noi','0123456789');

-- Chèn dữ liệu vào bảng OrderDetail
INSERT INTO OrderDetail (product_id, orders_id, product_amount)
VALUES 
(1, 1, 1),
(2, 2, 1),
(3, 3, 1),
(4, 4, 2),
(5, 5, 1),
(6, 6, 1),
(7, 7, 1),
(8, 8, 1),
(9, 9, 1),
(10, 10, 1);

-- Chèn dữ liệu vào bảng Plan
INSERT INTO Plan (plan_name, description, duration_week)
VALUES 
('3days', 'Kế hoạch tập 3 ngày mỗi tuần', 8),
('4days', 'Kế hoạch tập 4 ngày mỗi tuần', 8),
('5days', 'Kế hoạch tập 5 ngày mỗi tuần', 8);

-- delete from PlanDetail;
-- INSERT INTO PlanDetail (plan_id, customer_id, start_date, end_date)
-- VALUES 
-- (1, 1, '2024-11-04', '2025-01-01'),  -- Kế hoạch 3 ngày, cho khách hàng 1
-- (2, 2, '2024-11-04', '2025-01-01'),  -- Kế hoạch 4 ngày, cho khách hàng 2
-- (3, 3, '2024-11-04', '2025-01-01');  -- Kế hoạch 5 ngày, cho khách hàng 3


INSERT INTO Typeworkout (workoutday_type, description)
VALUES 
('Push', 'Ngày tập cơ ngực và vai'),
('Pull', 'Ngày tập cơ lưng và tay'),
('Leg', 'Ngày tập chân và mông'),
('Fullbody', 'Ngày tập toàn thân');


-- Chèn dữ liệu vào bảng Exercise cho các buổi tập
INSERT INTO Exercise (exercise_name, description, linkvideo, reps)
VALUES 
-- Các bài tập cho ngày Push
('Bench Press', 'Bài tập ngực với tạ đòn', 'https://www.youtube.com/embed/SCVCLChPQFY', 10),
('Shoulder Press', 'Bài tập vai với tạ đòn', 'https://www.youtube.com/embed/5yWaNOvgFCM', 10),
('Chest Fly', 'Bài tập ngực với máy', 'https://www.youtube.com/embed/eGjt4lk6g34', 12),
('Tricep Dips', 'Bài tập tay sau', 'https://www.youtube.com/embed/qrS6aa0aQ9I', 12),
('Push Ups', 'Chống đẩy', 'https://www.youtube.com/embed/0pkjOk0EiAk', 15),

-- Các bài tập cho ngày Pull
('Pull Ups', 'Bài tập kéo xà', 'https://www.youtube.com/embed/aAggnpPyR6E', 8),
('Barbell Row', 'Bài tập lưng với tạ đòn', 'https://www.youtube.com/embed/6FZHJGzMFEc', 10),
('Lat Pulldown', 'Bài tập lưng với máy kéo', 'https://www.youtube.com/embed/JGeRYIZdojU', 12),
('Dumbbell Row', 'Bài tập lưng với tạ tay', 'https://www.youtube.com/embed/6gvmcqr226U', 10),
('Bicep Curl', 'Bài tập tay trước với tạ', 'https://www.youtube.com/embed/HnHuhf4hEWY', 12),

-- Các bài tập cho ngày Leg
('Squats', 'Bài tập chân với tạ đòn', 'https://www.youtube.com/embed/QmZAiBqPvZw', 12),
('Leg Press', 'Bài tập đẩy chân', 'https://www.youtube.com/embed/qCR9bN3G1t4', 12),
('Lunges', 'Bài tập bước chân', 'https://www.youtube.com/embed/DlhojghkaQ0', 12),
('Leg Curls', 'Bài tập cơ chân sau', 'https://www.youtube.com/embed/SbSNUXPRkc8', 12),

('Barbell Curl', 'Bài tập tay trước với tạ đòn', 'https://www.youtube.com/embed/wyU4gwpOO3k', 12),  -- Tay trước
('Tricep Pushdown', 'Bài tập tay sau với máy kéo cáp', 'https://www.youtube.com/embed/LXkCrxn3caQ', 12), -- Tay sau
('Lat Pulldown', 'Bài tập lưng rộng với máy kéo', 'https://www.youtube.com/embed/JGeRYIZdojU', 10),  -- Lưng rộng
('Seated Row', 'Bài tập lưng dày với máy', 'https://www.youtube.com/embed/lJoozxC0Rns', 12),  -- Lưng dày
('Leg Press', 'Bài tập chân với máy đẩy', 'https://www.youtube.com/embed/qCR9bN3G1t4', 12);  -- Chân
-- Kế hoạch 1 (3, 4, hoặc 5 ngày)
-- Ngày Push: 5 bài tập, mỗi bài 3 set (tổng 15 set)

INSERT INTO ExerciseDetail (typeworkout_id, exercise_id, workout_sets)
VALUES 
-- Ngày Push
(1, 1, 3), -- Bench Press
(1, 2, 3), -- Shoulder Press
(1, 3, 3), -- Chest Fly
(1, 4, 3), -- Tricep Dips
(1, 5, 3); -- Push Ups

-- Chèn dữ liệu vào bảng ExerciseDetail cho các ngày Pull
-- Ngày Pull: 5 bài tập, mỗi bài 3 set (tổng 15 set)
INSERT INTO ExerciseDetail (typeworkout_id, exercise_id, workout_sets)
VALUES 
-- Ngày Pull
(2, 6, 3), -- Pull Ups
(2, 7, 3), -- Barbell Row
(2, 8, 3), -- Lat Pulldown
(2, 9, 3), -- Dumbbell Row
(2, 10, 3); -- Bicep Curl

-- Chèn dữ liệu vào bảng ExerciseDetail cho các ngày Leg
-- Ngày Leg: 4 bài tập, mỗi bài 3 set (tổng 12 set)
INSERT INTO ExerciseDetail (typeworkout_id, exercise_id, workout_sets)
VALUES 
-- Ngày Leg
(3, 11, 3), -- Squats
(3, 12, 3), -- Leg Press
(3, 13, 3), -- Lunges
(3, 14, 3); -- Leg Curls

-- Chèn dữ liệu vào bảng ExerciseDetail cho các ngày Fullbody
-- Ngày Fullbody: 5 bài tập, mỗi bài 3 set (tổng 15 set)
INSERT INTO ExerciseDetail (typeworkout_id, exercise_id, workout_sets)
VALUES 
-- Ngày Fullbody
(4, 15, 3), -- Deadlift
(4, 16, 3), -- Burpees
(4, 17, 3), -- Clean and Press
(4, 18, 3), -- Kettlebell Swing
(4, 19, 3); -- Mountain Climbers

-- Trigger dùng để generate ra các ngày tập nếu người dùng bắt đầu đăng kí một Plan
CREATE OR REPLACE FUNCTION generate_workout_details() RETURNS TRIGGER AS $$
DECLARE
    v_date DATE := NEW.start_date;  -- Ngày bắt đầu lấy từ PlanDetail
    v_count INT := 0;  -- Đếm số buổi tập
BEGIN
    -- Kiểm tra giá trị plan_id trong PlanDetail
    RAISE NOTICE 'Trigger activated for plan_id: %', NEW.plan_id;

    -- Kế hoạch 3 ngày mỗi tuần (Push, Pull, Leg, nghỉ xen kẽ)
    IF NEW.plan_id = 1 THEN  
        WHILE v_date <= NEW.end_date LOOP
            CASE v_count % 3
                WHEN 0 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (1, NEW.plandetail_id, v_date, 'Push day');
                WHEN 1 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (2, NEW.plandetail_id, v_date, 'Pull day');
                WHEN 2 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (3, NEW.plandetail_id, v_date, 'Leg day');
            END CASE;
            v_count := v_count + 1;
            v_date := v_date + INTERVAL '2 days';  -- Nghỉ 1 ngày giữa các buổi tập
        END LOOP;

    -- Kế hoạch 4 ngày mỗi tuần (Push, Pull, Leg, Fullbody, nghỉ xen kẽ)
    ELSIF NEW.plan_id = 2 THEN  
        WHILE v_date <= NEW.end_date LOOP
            CASE v_count % 4
                WHEN 0 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (1, NEW.plandetail_id, v_date, 'Push day');
                WHEN 1 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (2, NEW.plandetail_id, v_date, 'Pull day');
                WHEN 2 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (3, NEW.plandetail_id, v_date, 'Leg day');
                WHEN 3 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (4, NEW.plandetail_id, v_date, 'Fullbody day');
            END CASE;
            v_count := v_count + 1;
            v_date := v_date + INTERVAL '2 days';  -- Nghỉ 1 ngày giữa các buổi tập
        END LOOP;

    -- Kế hoạch 5 ngày mỗi tuần (Push, Pull, Leg, nghỉ, Push, Pull)
    ELSIF NEW.plan_id = 3 THEN  
        WHILE v_date <= NEW.end_date LOOP
            CASE v_count % 5
                WHEN 0 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (1, NEW.plandetail_id, v_date, 'Push day');
                WHEN 1 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (2, NEW.plandetail_id, v_date, 'Pull day');
                WHEN 2 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (3, NEW.plandetail_id, v_date, 'Leg day');
                WHEN 3 THEN
                    v_date := v_date + INTERVAL '1 day';  -- Nghỉ 1 ngày ở giữa
                WHEN 4 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (1, NEW.plandetail_id, v_date, 'Push day');
                WHEN 5 THEN
                    INSERT INTO WorkoutDetail (typeworkout_id, plandetail_id, date_workout, description)
                    VALUES (2, NEW.plandetail_id, v_date, 'Pull day');
            END CASE;
            v_count := v_count + 1;
            v_date := v_date + INTERVAL '1 day';  -- Tiếp tục lặp lại chu kỳ
        END LOOP;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

--Trigger dùng để update lại thông tin calories còn lại trong dailydiary mỗi khi thêm hay xóa thông tin FoodDetail tương ứng 
CREATE OR REPLACE FUNCTION update_calories_remain()
RETURNS TRIGGER AS $$
DECLARE
    food_calories INT;
    total_calories_consumed DECIMAL(10, 2);
BEGIN
    IF (TG_OP = 'INSERT') THEN
        SELECT calories INTO food_calories FROM Food WHERE food_id = NEW.food_id;
        total_calories_consumed := food_calories * NEW.food_amount;

        UPDATE DailyDiary
        SET calories_remain = calories_remain - total_calories_consumed
        WHERE dailydiary_id = NEW.dailydiary_id;

    ELSIF (TG_OP = 'DELETE') THEN
        SELECT calories INTO food_calories FROM Food WHERE food_id = OLD.food_id;
        total_calories_consumed := food_calories * OLD.food_amount;

        UPDATE DailyDiary
        SET calories_remain = calories_remain + total_calories_consumed
        WHERE dailydiary_id = OLD.dailydiary_id;

    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

--Trigger cập nhật lại daily_weight trên DailyDiary khi cập nhật lại thông tin weight  trên customer_health
CREATE OR REPLACE FUNCTION update_daily_weight()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE DailyDiary
    SET daily_weight = NEW.weight
    WHERE customer_id = NEW.customer_id
      AND diary_date = CURRENT_DATE;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

--Kích hoạt trigger
DROP TRIGGER IF EXISTS trg_generate_workout_details ON PlanDetail;
DROP TRIGGER IF EXISTS trg_update_calories_remain ON FoodDetail;
DROP TRIGGER IF EXISTS trg_update_daily_weight ON Customer_Health;

CREATE TRIGGER trg_generate_workout_details
AFTER INSERT ON PlanDetail
FOR EACH ROW
EXECUTE FUNCTION generate_workout_details();

CREATE TRIGGER trg_update_calories_remain
AFTER INSERT OR DELETE ON FoodDetail
FOR EACH ROW
EXECUTE FUNCTION update_calories_remain();

CREATE TRIGGER trg_update_daily_weight
AFTER UPDATE ON Customer_Health
FOR EACH ROW
EXECUTE FUNCTION update_daily_weight();

-- Test trigger
-- INSERT INTO PlanDetail (plan_id, customer_id, start_date, end_date)
-- VALUES (1, 1, '2024-11-12', '2025-01-07');

select * from PlanDetail;
SELECT * FROM WorkoutDetail;
select * from ExerciseDetail;
select * from exercise;
select * from TypeWorkout;
select * from FoodDetail;
select * from DailyDiary;
select * from Customer;
select * from orders;
select * from payment;
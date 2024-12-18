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
    total_price DECIMAL(10, 2),
	status TEXT
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
('Whey Protein', 'Bổ sung protein cho cơ bắp.'),
('Pre-Workout', 'Tăng năng lượng trước tập luyện.'),
('Creatine', 'Hỗ trợ sức mạnh và sức bền.'),
('Vitamin', 'Cung cấp vitamin cho cơ thể.'),
('Khoáng chất', 'Cung cấp khoáng chất thiết yếu.'),
('Gia vị', 'Thêm hương vị và dinh dưỡng cho bữa ăn.'),
('Phụ kiện thể thao', 'Thiết bị hỗ trợ tập luyện.');

-- Chèn dữ liệu vào bảng Product
INSERT INTO Product (category_id, product_name, product_price, product_provider, description)
VALUES 
--Whey Protein
(1, 'Repp Sports Raze Hydro Whey 5 Lbs (2.3kg)',1980000, 'Repp Sports', 'Sản phẩm cung cấp 27g protein mỗi khẩu phần, 100% Hydrolyzed Whey Isolate, bổ sung enzyme hỗ trợ tiêu hóa, phù hợp cho người không dung nạp lactose. Ít đường, ít calo, phù hợp với người ăn kiêng. Hương vị thơm ngon, không ngấy.'),
(1, 'Rule 1 Protein 5 Lbs (2,288 kg)', 1590000, ' Rule One Proteins', 'Whey Protein Isolate & Hydrolyzate cung cấp 25g protein mỗi khẩu phần (tỷ lệ 83.3%), không chứa đường, lactose, gluten, và không sử dụng Amino Spiking. Sản phẩm có 76 khẩu phần, với đa dạng hương vị để lựa chọn.'),
(1, 'PVL ISO Gold - Premium Whey Protein With Probiotic, 5 Lbs (2.27kg)', 2050000, 'PVL', 'Sản phẩm cung cấp 27g protein từ Whey Isolate & Hydrolysate, 100% nguồn gốc từ bò ăn cỏ chăn thả tự nhiên. Chứa 17g EAA, BCAA và Glutamine, cùng 1 tỷ lợi khuẩn Probiotic hỗ trợ tiêu hóa. Phù hợp cho người không dung nạp lactose và đạt chứng nhận GMP, Inform-Choice.'),
(1, 'Nutrabolics Hydropure 4.5 Lbs, 58 Servings', 1890000, 'Nutrabolic', 'Sản phẩm 100% Hydrolyzed Whey Protein cung cấp 28g protein, 13g EAA, 6.1g BCAA, và 5g Glutamine. Không chứa lactose, gluten và đạt chứng chỉ HACCP, GMP, ISO, HALAL'),
(1, 'DY Shadowhey Hydrolysate 2.27Kg (90 Servings)',1750000, 'DY Nutrition', 'Sản phẩm Whey Isolate và Whey Hydrolyzed cung cấp 23g protein/serving (tỷ lệ 90%), 6g BCAA, 12g EAA. Bổ sung HMB, Axit D-Aspartic và kẽm hỗ trợ hiệu suất tập luyện và testosterone, cùng enzyme lactase hỗ trợ tiêu hóa. Chỉ 96 calo, không carb, không fat, không đường, phù hợp với người không dung nạp lactose và gluten'),
(1, 'BioTechUSA Hydro Whey Zero', 1990000, 'Biotech USA', 'Sản phẩm Whey Hydrolyzed & Whey Isolate cung cấp 18g protein, 3.7g BCAA, 7.5g EAA, và 4g Glutamine, chỉ với 85 calo/serving. Phù hợp với người không dung nạp lactose và gluten, không gây nổi mụn'),
(1, 'ON Platinum Hydrowhey, 3.5 Lbs (1.59 kg)', 1590000, 'Optimum Nutrition', 'Sản phẩm Whey Hydrolyzed & Whey Isolate cung cấp 18g protein, 3.7g BCAA, 7.5g EAA, và 4g Glutamine, chỉ với 85 calo/serving. Phù hợp với người không dung nạp lactose và gluten, không gây nổi mụn'),
(1, 'Mutant ISO Surge 5 Lbs (76 Servings)', 1850000, 'Mutant', 'Sản phẩm Whey Protein Isolate & Hydrolyzate cung cấp 25g protein phát triển cơ bắp, 16.3g EAAs tổng hợp, 5.5g BCAA tăng sức bền tập luyện, và 4.6g Glutamine phục hồi cơ bắp. Bổ sung enzyme hỗ trợ tiêu hóa, phù hợp với người không dung nạp lactose'),
(1, 'Biotech USA ISO Whey Zero 5 Lbs (2,270 Kg)', 1950000, 'Biotech USA', 'Sản phẩm cung cấp 22g Protein Isolate, 9.5g EAAs, 4.5g BCAA và 4.3g Glutamine. Hương vị dễ uống'),
(1, 'ON Whey Gold Standard 100% Whey Protein, 5 Lbs', 1590000, 'Optimum Nutrition', 'Sản phẩm bán chạy nhất thế giới 9 năm liên tiếp, cung cấp 24g protein, 5.5g BCAA và 4g Glutamine. Đa dạng hương vị để lựa chọn'),
(1, 'AllMax IsoFlex 5Lbs (2,27 kg)', 2150000, 'AllMax Nutrition', 'Sản phẩm cung cấp 27g Whey Protein Isolate, 6g BCAA và 4.6g Glutamine. Bổ sung enzyme hỗ trợ tiêu hóa tốt hơn, không chứa fat, đường, chất xơ, gluten và đậu nành'),
(1, 'Kevin Levrone GOLD ISO - Pure Whey Protein Isolate, 2 KG (60 Servings)', 1500000, 'Kevin Levrone', 'Sản phẩm 100% Whey Protein Isolate cung cấp 27g protein và 7.5g BCAA, không chứa fat và đường. Phù hợp cho người không dung nạp lactose'),
(1, 'PVL ISO Gold - Premium Whey Protein With Probiotic, 2 Lbs (908 gram)', 1050000, 'PVL', 'Sản phẩm cung cấp 27g protein từ Whey Protein Isolate & Hydrolysate, 17g EAA, BCAA và Glutamine, 100% nguồn gốc từ bò ăn cỏ. Bổ sung 1 tỷ probiotic lợi khuẩn bảo vệ sức khỏe đường ruột và enzyme hỗ trợ tiêu hóa. Đạt chứng nhận GMP và Inform-Choice.'),
(1, 'Applied ISO XP Whey Protein Isolate, 1.8 KG (72 Servings)', 1790000, 'Applied Nutrition', 'Sản phẩm cung cấp 22.4g protein (tỷ lệ 90%, cao nhất trong phân khúc), 100% Whey Protein Isolate, đạt chứng nhận cGMP, Halal, ISO 22000 và BRC. Không chứa lactose, gluten và đậu nành'),
(1, 'Rule 1 Protein 10 Lbs (4,576 kg)', 2990000, 'Rule 1 Proteins', 'Sản phẩm 100% Whey Protein Isolate & Hydrolyzate cung cấp 25g protein (tỷ lệ 83.3%), không chứa đường, lactose, gluten và không sử dụng Amino Spiking. Có 152 khẩu phần và đa dạng hương vị để lựa chọn'),
(1, 'Rule 1 Whey Blend 5 Lbs (2.3Kg) - 70 Servings', 1250000, 'Rule 1 Proteins', 'Sản phẩm cung cấp 24g protein và 5.5g BCAAs, không sử dụng Spiking (chất tạo hàm lượng protein giả tạo), không chất cấm, không chứa gluten'),
(1, 'Applied ISO XP Whey Protein Isolate, 1 KG (40 Servings)', 1050000, 'Applied Nutrition', 'Sản phẩm cung cấp 22.4g protein (tỷ lệ 90%, cao nhất trong phân khúc), 100% Whey Protein Isolate, đạt chứng nhận cGMP, Halal, ISO 22000 và BRC. Không chứa lactose, gluten và đậu nành'),
(1, 'Applied Clear Whey Protein Hydrolysed 875G (35 Servings)',1050000, 'Applied Nutrition', 'Sản phẩm 100% Hydrolyzed Whey Protein cung cấp 21g protein và 5g BCAA, chỉ 90 calo/serving. Hàm lượng đường và chất béo thấp, hương vị trái cây tự nhiên thanh mát, khử vị sữa. Đạt chứng nhận Halal'),
(1, 'Amix Gold Isolate Whey Protein, 5 Lbs (76 Servings)',1750000,'Amix Nutrition', 'Sản phẩm bổ sung 26g protein với tỷ lệ protein lên tới 86%, sử dụng công nghệ lọc ép lạnh CFM tiên tiến nhất. Chứa 106 calo, 0.1g fat, 0.3g carbohydrate và ít đường'),
(1, 'Scitec Nutrition 100% Whey Protein Isolate, 2000 Gams (80 Servings)',2050000, 'Scitec Nutrition', 'Sản phẩm 100% Whey Protein Isolate, bổ sung L-Glutamine và L-Arginine, với lượng calo thấp, không đường, không chứa gluten và không có dầu cọ'),
(1, 'Perfect Diesel Whey Isolate New Zealand, 5 Lbs (75 Servings)',2250000, 'Perfect Sports', 'Sản phẩm cung cấp 27g protein (tỷ lệ 90%), 13g EAA, 7g BCAA, được làm từ sữa bò New Zealand chất lượng cao. Không chứa lactose, gluten, carb, đường và cholesterol. Không gây nóng trong, nổi mụn hay rối loạn tiêu hóa. Hương vị dịu nhẹ, dễ uống'),
(1, 'MyProtein Impact Whey Protein, 2.5 Kg (100 Servings)',1750000, 'MyProtein', 'Sản phẩm cung cấp 18g-20g protein hấp thụ nhanh chóng, 4.5g BCAA, 2g Leucine và 3.6g Glutamine mỗi lần dùng. Đạt vị trí TOP đầu trong chứng nhận Labdoor'),
(1, 'ON Gold Standard 100% Whey Protein 2 Lbs (908 g)',950000, 'Optimum Nutrition', 'Sản phẩm bán chạy nhất thế giới 9 năm liên tiếp, cung cấp 24g protein, 5.5g BCAA và 4g Glutamine. Đa dạng hương vị để lựa chọn'),
(1, 'Dymatize ISO 100 Hydrolyzed, 5 Lbs (2.27 kg)',2390000, 'Dymatize Nutrition', 'Sản phẩm cung cấp 25g Hydrolyzed 100% Whey Protein Isolate, 5.5g BCAAs, 2.6g L-Leucine, không chứa fat, gluten và lactose.'),
(1, 'ON Gold Standard 100% Whey Protein 10 Lbs (4.54 kg)',2960000, 'Optimum Nutrition', 'Sản phẩm bán chạy nhất thế giới 9 năm liên tiếp, cung cấp 24g protein, 5.5g BCAA và 4g Glutamine. Đa dạng hương vị để lựa chọn'),
(1, 'MyProtein Impact Whey Protein, 2.5 Kg (100 Servings)',1750000, 'MyProtein', 'Sản phẩm cung cấp 18g-20g protein hấp thụ nhanh chóng, 4.5g BCAA, 2g Leucine và 3.6g Glutamine mỗi lần dùng. Đứng vị trí TOP đầu trong chứng nhận Labdoor'),
(1, 'BPI ISO HD 100% Pure Isolate Protein, 5 Lbs (69 Servings)',1540000, 'BPI Sport', 'Sản phẩm 100% Whey Protein Isolate cung cấp 25g protein, 120 calo, chỉ 1g fat và đường. Hương vị thơm ngon, bột mịn dễ tan'),
(1, 'PVL Whey Gold - 100% Whey Protein Shake Mix, 6Lbs (72 Servings)',1640000, 'PVL', 'Sản phẩm cung cấp 24g protein từ 100% bò ăn cỏ, 15g EAA, không thêm đường, bổ sung MCT Oil Power (chất béo tốt dễ hấp thụ), đường cỏ ngọt Stevia và enzyme tiêu hóa'),
(1, 'Nutrex ISOFIT, 5 Lbs(70 Servings)',1550000, 'Nutrex', 'Sản phẩm 100% Whey Protein Isolate cung cấp 25g protein, 12.2g EAA, 5.9g BCAA và dưới 1g đường'),
(1, 'MuscleTech NITRO-TECH, 10 Lbs (4,54 Kg)',2850000, 'MuscleTech', 'Sản phẩm cung cấp 30g protein, 3g Creatine Monohydrate, 6.9g BCAA và 5.3g Glutamine'),
--Pre-workout
(2, 'PVL Domin8 Pre-workout Superfuel, 520 Gams (40 Servings)',1050000, 'PVL', 'Sản phẩm bổ sung 4g Citrulline Malate, 1.6g Beta-Alanine, 300mg Taurine, 150mg Caffeine Anhydrous, cùng L-Tyrosine và Caffeine Citrate. Tăng năng lượng, cải thiện sự tỉnh táo và tập trung, tăng sức mạnh, sức bền, pump cơ cực đại và kích thích phục hồi cơ bắp'),
(2, 'Applied Nutrition ABE PUMP | Stim Free Formula, 40 Servings',950000, 'Applied Nutrition', 'Sản phẩm phiên bản không caffeine, không gây mất ngủ, bổ sung Citrulline Malate, Beta Alanine, Creatine, Taurine, Arginine, và hợp chất FitNox® giúp gia tăng mức NO lên tới 336%. Tăng pump cơ siêu phồng, sức mạnh, sức bền, cải thiện hiệu suất tập, hạn chế đau nhức, chuột rút và bổ sung muối hồng cùng kali để bù điện giải'),
(2, 'Applied ABE Pre-Workout, 30 Servings',720000, 'Applied Nutrition', 'Sử dụng trước khi tập, hỗ trợ tăng sức mạnh với Tri-Creatine 3,25g, Beta Alanine 2g, Citrulline Malate 4g, Teacrine 100mg và Caffeine 200mg. Tăng lượng Nitric Oxide giúp bơm căng cơ và mang lại cảm giác ngứa cực điên'),
(2, 'Applied PUMP 3G Pre-Workout | With Caffein, 50 Scoops',650000, 'Applied Nutrition', 'Sản phẩm giúp tăng tập trung, tỉnh táo, giảm mệt mỏi và buồn ngủ. Thúc đẩy mức năng lượng, gia tăng sức mạnh, cải thiện sức bền, sức chịu đựng để hoàn thành bài tập khó. Bơm cơ căng phồng, phù hợp cho bulking, hỗ trợ xây dựng cơ bắp, tăng trao đổi chất và hỗ trợ đốt mỡ giảm cân. Sản phẩm đạt chứng nhận ISO 22000, BRC, GMP và Halal.'),
(2, 'APS Pre-workout Mesomorph, 25 Servings',950000, 'APS', 'Sản phẩm chứa Creatine Nitrate, Creatinol-O-Phosphate, 3.2g Beta-Alanine, Citrulline Malate, Agmatine Sulfate, Taurine và Methylxanthine Anhydrous (DMAA) giúp tăng sức mạnh, bơm cơ, cải thiện hiệu suất tập luyện và tăng năng lượng'),
(2, 'Cobra Labs The Curse, 50 Servings',530000, 'JNX Sports', 'Sản phẩm giúp tăng sức bền, sức chịu đựng, giảm mệt mỏi cơ bắp, thúc đẩy lưu thông máu và giúp bạn tỉnh táo, hưng phấn hơn khi tập luyện. Không gây chóng mặt, buồn nôn'),
(2, 'BSN N.O.-Xplode, 60 Servings',1650000, 'BSN', 'Sản phẩm chứa 3g Creatine Monohydrate, 1.8g Beta-Alanine, 275mg Caffeine, cùng các vitamin và khoáng chất điện giải. Bùng nổ năng lượng, tăng sức mạnh, sức bền, và có hương vị thơm ngon'),
(2, 'Evogen EVP Xtreme, 24 Servings',1400000, 'Evogen', 'Bột Pre-Workout giúp tăng cường pump và tập trung tối đa, với công thức bao gồm Caffeine Anhydrous, Choline Bitartrate và Piper Nigrum Extract'),
(2, 'Mutant Pre-workout Madness, 30 Servings',700000, 'Mutant', 'Sản phẩm giúp tăng tỉnh táo trước tập, giảm mệt mỏi, tối đa hóa hiệu suất tập luyện, chống mỏi cơ. Dễ sử dụng và có đa dạng hương vị'),
(2, 'Blackstone Labs Dust X with DMHA Pre Workout, 25 Servings',950000, 'Blackstone Labs', 'Sản phẩm chứa 150mg 2-aminoisoheptane, cung cấp năng lượng mạnh mẽ, tập trung cao độ, bơm cơ căng phồng và công thức kích thích mạnh mẽ nhất'),
(2, 'Metabolic Nutrition E.S.P Pre-Workout, 90 servings',800000, 'Metabolic Nutritions', 'Pre-Workout Amplifier với Agmatine Sulfate giúp cải thiện hiệu suất tập luyện, với thành phần chính là Beta-Alanine'),
(2, 'Muscle Sport Rhino Rampage Pre Workout, 30 Servings (210 gram)',550000, 'Muscle Sport', 'Sản phẩm giúp tăng sức bền tập luyện, thúc đẩy năng lượng hiệu quả, bơm cơ tốt hơn và tăng cường sự tập trung với 7 thành phần độc quyền'),
(2, 'Nutrex Outlift Concentrate, 30 Servings',650000, 'Nutrex', 'Sản phẩm tăng sức mạnh và sức bền, cải thiện tập trung, giảm mệt mỏi, uể oải. Sử dụng công nghệ N03-T8 Nitrate, gia tăng Glycogen và có hương vị thơm ngon'),
(2, 'CBUM Thavage Pre Workout - 40 Servings',1400000, 'Cbum', 'Sản phẩm giúp gia tăng sức mạnh cơ bắp, tăng độ pump của cơ, cải thiện sự tập trung trong tập luyện, hỗ trợ bù nước và khoáng. Chứa 3200mg Beta Alanine, 2500mg Betaine Anhydrous, 3000mg Creatine Monohydrate, 6000mg L-Citrulline, 2000mg Taurine, 260mg Caffeine Anhydrous, và 63mg Di Caffeine Malate (45mg tác dụng)'),
(2, 'Outbreak Pathogen Apocalyptic Pre-Workout, 325g (25 Servings)',630000, 'Outbreak Pathogen', 'Sản phẩm chứa 3200mg Beta Alanine, 6000mg Citrulline, 100mg Theobromine, 350mg Caffeine, 500mg L-Tyrosine, 500mg Nelumbo Nucifera (Sen Hoa) và Chiết xuất trà xanh. Công thức này hỗ trợ tăng sức mạnh, sức bền, cải thiện sự tập trung và giảm mệt mỏi trong khi tập luyện.'),
(2, 'BSN Nitrix 2.0 Advanced Strength',750000, 'BSN', 'Sản phẩm tiền chất Nitric Oxide cô đặc, giúp thúc đẩy sự bơm cơ, phục hồi và cải thiện hiệu suất tập luyện'),
(2, 'Cellucor C4 Ultimate Pre-workout , 20 Servings',950000, 'Cellucor', 'Sản phẩm này chứa 3.2g Carnosyn Beta Alanine, 1.5g Super Compound Creatine Nitrate, 300mg Caffeine, 6g Citrulline Malate, 1.5g Betaine Anhydrous và 1.5g NitroSigine, giúp tăng sức mạnh, sức bền, bơm cơ và cải thiện sự tập trung khi luyện tập'),
(2, 'Pre-workout Pre JYM, 20 Servings',890000, 'JYM', 'Sản phẩm này chứa 6g Citrulline Malate, 6g BCAA tỉ lệ 2:1:1, 2g Creatine HCl (Con-Cret®), 2g Beta-Alanine, 1.5g Betaine, 500mg Beet Root Extract, 300mg Caffeine, 150mg Alpha-GPC, 50mcg Huperzine A, và 5mg BioPerine Black Pepper Extract, giúp tăng cường sức mạnh, bơm cơ, cải thiện sự tập trung và năng lượng trong khi luyện tập'),
(2, 'Primal Pre Workout, 30 Servings (447 Gams)',750000, 'Primal', 'Sản phẩm chứa 6g L-Citrulline, 3.3g Beta Alanine, và 1g Taurine, giúp tăng sức mạnh, sức bền và tối ưu hiệu suất tập luyện.'),
(2, 'Kevin Levrone ScatterBrain Pre Workout, 30 Servings',450000, 'Kevin Levrone', 'Sản phẩm giúp duy trì tỉnh táo, tập trung trong thời gian tập luyện, tăng cường sức mạnh và sức bền, gia tăng hiệu suất tập luyện, hỗ trợ phục hồi nhanh chóng và giảm tình trạng mệt mỏi trong quá trình tập'),
--Creatine
(3, 'Kevin Levrone GOLD Creatine Chews',360000, 'Kevin Levrone', 'Sản phẩm chứa 5g Creatine Monohydrate dạng kẹo, rất tiện lợi khi sử dụng. Với hương vị đặc biệt: thanh long và mâm xôi xanh, sản phẩm hỗ trợ phục hồi và phát triển cơ bắp, kích thích tổng hợp protein cơ bắp, hỗ trợ sức khỏe não bộ và gia tăng khối lượng cơ nạc'),
(3, 'Nutricost Creatine Monohydrate Powder Micronized',450000, 'Nutricost', 'Sản phẩm chứa 5g Creatine Monohydrate, giúp tăng sức mạnh, sức bền, kích thước và khối lượng cơ bắp. Nó còn hỗ trợ pump cơ, thúc đẩy tổng hợp protein, cải thiện chức năng não, và đạt chứng nhận GMP, non-GMO, gluten-free'),
(3, 'Ostrovit Creatine Monohydrate 500g',530000, 'Ostrovit', 'Sản phẩm cung cấp 3000mg Creatine Monohydrate, với 2640mg creatine tinh khiết, hỗ trợ phát triển cơ bắp, tăng cường sức bền và gia tăng hiệu suất tập luyện.'),
(3, 'Nutrex Micronized Creatine Monohydrate, 300g (60 Servings)',390000, 'Nutrex', 'Creatine Monohydrate tinh khiết giúp tăng cường khối lượng cơ, sức mạnh và sức bền. Được nghiên cứu kỹ lưỡng, đảm bảo an toàn và hiệu quả, sản phẩm không có hương vị, dễ dàng kết hợp với Pre-workout và BCAA'),
(3, 'Kevin Levrone GOLD Creatine Monohydrate',450000, 'Kevin Levrone', '5G Creatine Monohydrate bổ sung Vitamin B6 giúp tăng tỉnh táo và tập trung, gia tăng mức năng lượng cho cơ thể, kích thích tổng hợp protein cơ bắp, hỗ trợ sức khỏe não bộ và kiểm soát đường huyết'),
(3, 'VitaXtrong 100% Pure Creatine Monohydrate 5000, Unflavored',390000, 'VitaXtrong', '5g Creatine Monohydrate thúc đẩy sức mạnh, sức bền, tăng kích thước và khối lượng cơ, kích thích tổng hợp protein và cải thiện chức năng não'),
(3, 'Mutant Creakong Creatine, 249 Gams (30 Servings)',600000, 'Mutant', 'Công thức Creatine tiên tiến hỗ trợ kích thước cơ, sức mạnh và sức bền, với 3 dạng creatine đã được chứng minh lâm sàng cho kết quả tối đa. Thành phần được thử nghiệm trong phòng thí nghiệm đảm bảo hiệu quả'),
(3, 'Amix Creatine Monohydrate Creapure | Unflavored, 300g (100 Servings)',720000, 'Amix Nutrition', '3g Creatine Monohydrate Super Micronized với công thức Creapure tinh khiết 99.99% và kích thước phân tử giảm gần gấp 20 lần, giúp khuếch đại sức mạnh cơ bắp nhanh chóng và hiệu quả.'),
(3, 'MuscleTech CELL TECH Creactor, 120 Servings',640000, 'Muscle Tech', '750mg Creatine Hydrochloride (HCI) và 750mg Free-Acid Creatine giúp tăng sức mạnh, kích thích phát triển cơ bắp và nâng cao hiệu suất tập luyện'),
(3, 'Now Micronized Creatine Monohydrate Powder',690000, 'Now', '4.2g Creatine mỗi serving, được micronized hóa với kích thước phân tử nhỏ hơn 1/17 so với creatine thông thường, giúp bột siêu mịn, dễ tan và nhanh hấp thu. Sản phẩm không chứa gluten, non-GMO, phù hợp cho người ăn chay. Thúc đẩy sức mạnh, sức bền, pump phồng cơ, kích thích tổng hợp protein và cải thiện chức năng não'),
(3, 'PVL 100% Pure Creatine Unflavour, 300 gram (60 Servings)',450000, 'PVL', '5g Creatine Monohydrate giúp gia tăng sức mạnh cơ bắp, tăng sức bền và hiệu suất tập luyện. Nó giữ nước trong các sợi cơ, giúp tăng kích thước cơ bắp, đồng thời cải thiện sự tập trung trong quá trình tập luyện'),
(3, 'Scitec Nutrition Crea-Bomb, 660 grams (110 Servings)',1000000, 'Scitec Nutrition', '5g creatine, bao gồm 6 loại khác nhau, thúc đẩy năng lượng, tăng sức mạnh và sức bền. Nó hỗ trợ phát triển cơ bắp, đặc biệt phù hợp cho giai đoạn bulking.'),
(3, 'BPI Micronized Creatine, Unflavored',450000, 'BPI', '5G Creatine giúp tăng sức mạnh cơ bắp, kích thích sản sinh năng lượng và hấp thu cực nhanh. Sản phẩm không vị, dễ kết hợp với các đồ uống khác'),
(3, 'JNX Sports The Curse Micronized Creatine Monohydrate',390000, 'JNX Sports', '5g Micronized Creatine Monohydrate mỗi serving, không chất phụ gia, không vị và dễ pha trộn. Sản phẩm thân thiện với người chay, giúp tăng sức mạnh, pump cơ và nâng cao hiệu suất tập luyện'),
(3, 'FA ICE Creatine Monohydrate, 300G (60 Servings)',550000, 'FA Engineered Nutrition', 'Phát triển khối lượng cơ nạc, tăng thể tích tế bào cơ, hỗ trợ phục hồi cơ bắp và giảm cortisol. Sản phẩm ức chế quá trình dị hóa cơ bắp, gia tăng sức mạnh tổng thể, cải thiện sức bền tập luyện và hỗ trợ chức năng của não bộ'),
(3, 'My Protein Creatine Monohydrate, 500g',550000, 'MyProtein', 'Tăng cường hiệu suất thể chất, creatine không chứa chất độn hoặc phụ gia. Mục tiêu hỗ trợ: Xây dựng cơ bắp. Thành phần chính: Creatine Monohydrate'),
(3, 'Elite Labs USA Metabolic Creatine 6, 30 Servings',490000, 'Elite Labs', 'Sản phẩm kết hợp 6 loại creatine khác nhau, giúp thúc đẩy sức mạnh, sức bền, pump phồng cơ, kích thích tổng hợp protein và tăng cường chức năng não'),
(3, 'Rule1 Charged Creatine, 30 Servings (240 Gams)',490000, 'Rule 1 Proteins', 'Sản phẩm chứa 5g hợp chất Creatine từ 3 nguồn khác nhau, kết hợp với 150mg từ trà, cà phê, Guarana và Mate, cùng 120mg caffeine nguồn tự nhiên, giúp tăng cường năng lượng, sức mạnh và tập trung'),
(3, 'PVL Creatine X8 | Unflavour, 240 gams (30 Servings)',650000, 'PVL', 'Sản phẩm này giúp tăng cường hiệu suất tập luyện tối đa với Creatine Monohydrate hấp thụ nhanh, công thức CSM hỗ trợ tăng cường sản xuất Creatine tự nhiên và bổ sung BCAA giúp tăng khả năng tổng hợp protein. Không có màu sắc hoặc hương vị nhân tạo, công thức sạch, chất lượng, được kiểm định bởi Informed Choice và đạt tiêu chuẩn GMP'),
(3, 'Biotech 100% Creatine Monohydrate, Unflavoured',600000, 'Biotech USA', 'BioTech 100% CREATINE MONOHYDRATE dạng bột với độ tinh khiết cao giúp tăng sức mạnh, tăng hiệu suất của bạn trong một loạt các bài tập cường độ cao ngắn hạn.'),
--Vitamin
(4, 'Multivitamin cao cấp cho nam - NOW Mens Active Sports Multi',590000, 'Now', 'Sản phẩm này cung cấp vitamin, khoáng chất thiết yếu, bổ sung amino axit gốc tự do và 8 loại chiết xuất thảo mộc, trái cây. Nó hỗ trợ cải thiện năng lượng, tăng cường hệ thống miễn dịch, sức khỏe tim mạch, và giúp giảm thiểu cholesterol xấu. Đóng gói 90/180 viên, từ thương hiệu Now Foods'),
(4, 'ON Opti-Men, Multivitamin for Active Men',700000, 'Optimum Nutrition', 'Sản phẩm này chứa hơn 75 hoạt chất, bao gồm 25 vitamin và khoáng chất, 1000mg axit amin dạng tự do và 30 chiết xuất từ thiên nhiên. Nó hỗ trợ tăng sức đề kháng, xây dựng và phục hồi cơ bắp hiệu quả.'),
(4, 'Universal Nutrition Animal Pak, 44 Packs',1100000, 'Universal Nutrition', 'Sản phẩm này cung cấp 22 vitamin và khoáng chất, 8 axit amin, giúp tăng cường hệ miễn dịch, hỗ trợ phát triển cơ bắp và thúc đẩy phục hồi sau tập luyện. Đặc biệt, nó đã 6 năm liền đạt Top 1 Multi-Vitamin trên Bodybuilding'),
(4, 'GAT Sport Mens Multi+Test, 60 Tablets',450000, 'GAT Sports', 'Sản phẩm này chứa hơn 40 hoạt chất mỗi viên, hỗ trợ tăng cường Testosterone tự nhiên với Boron và Tribulus, giúp tăng cường hệ miễn dịch khỏe mạnh và cung cấp dinh dưỡng hoàn chỉnh cho vận động viên.'),
(4, 'ON Opti-Women, 60 Capsules',480000, 'Optimum Nutrition', 'Sản phẩm chứa 40 thành phần hoạt tính, bao gồm 17 thành phần đặc biệt và 23 vitamin & khoáng chất. Mỗi viên bổ sung 150mg Canxi, 18mg Sắt và 600 microgam Folate, hỗ trợ sức khỏe tổng thể và phục hồi cơ thể'),
(4, 'Bronson Vitamin K2 + D3, Vitamin K2 as MK-7',250000, 'Bronson', 'Sản phẩm giúp cải thiện sức khỏe, tăng cường sức đề kháng và miễn dịch hiệu quả. Nó thúc đẩy xương chắc khỏe, hỗ trợ phục hồi chấn thương, tăng cường sinh lý nam và chống xơ vữa động mạch. Đồng thời, giúp hấp thụ canxi hiệu quả và giảm nguy cơ mắc bệnh đột quỵ'),
(4, 'Ostrovit Vitamin D3 4000IU + K2, 110 Tablets',180000, 'Ostrovit', 'Sản phẩm chứa 4000 IU Vitamin D3 và 100 mcg Vitamin K2 MK7, giúp tăng cường sức khỏe hệ xương, hỗ trợ tim mạch, cải thiện tâm trạng và ngăn ngừa bệnh tiểu đường. Nó cũng nâng cao sức đề kháng và hỗ trợ quá trình tập luyện'),
(4, 'Now Mega D-3 & MK-7, 60 Veg Capsules',400000, 'Now', 'Sản phẩm chứa 5000 IU Vitamin D3 và 180 mcg Vitamin K2 (MK7), giúp thúc đẩy xương chắc khỏe, bảo vệ sức khỏe tim mạch, thận và não bộ. Nó cũng hỗ trợ tăng cường testosterone tự nhiên, cân bằng mức hormone và nâng cao sức đề kháng'),
(4, 'Natural Vitamin E-400 With Mixed Tocopherols, 250 Softgels',750000, 'Now', 'Sản phẩm giúp chống lão hóa, ngăn ngừa nếp nhăn, hỗ trợ ngăn ngừa hiệu quả bệnh tim mạch và cải thiện sức khỏe hệ thần kinh.'),
(4, 'Now Foods Vitamin E-400 Vegetarian Dry Veg Capsules, 100 VCaps',405000, 'Now', 'Sản phẩm Vitamin E tự nhiên - 400 IU với hỗn hợp Tocopherols, đóng gói 100 viên mềm. Vitamin E giúp bảo vệ tế bào khỏi sự tổn thương do các gốc tự do, hỗ trợ sức khỏe tim mạch, cải thiện chức năng miễn dịch và duy trì làn da khỏe mạnh'),
(4, 'Nutricost Vitamin B Complex 460 MG',350000, 'Nutricost', 'Sản phẩm bổ sung hỗn hợp 8 vitamin B (B1, B2, B3, B5, B6, B7, B9, B12) với dạng hoạt chất hấp thu nhanh, hàm lượng vitamin B cao vượt trội. Bổ sung thêm vitamin C để tăng cường miễn dịch, giúp giảm mệt mỏi, căng thẳng, cải thiện trí nhớ và tăng cường khả năng miễn dịch'),
(4, 'NOW Vitamin B-50 mg, Energy Production, Nervous System Health',450000, 'Now', 'Sản phẩm bổ sung 8 loại vitamin B (B1, B2, B3, B5, B6, B7, B9, B12), cùng Choline, Inositol (Vitamin B8) và PABA (Vitamin B10). Hỗ trợ trí não, chức năng thần kinh, giảm stress, căng thẳng, ổn định tâm trạng, tăng mức năng lượng và hỗ trợ hiệu quả cho tập luyện và vóc dáng'),
(4, 'OstroVit Vitamin B Complex, 90 Tablets',250000, 'Ostrovit', 'Sản phẩm bổ sung hỗn hợp 8 vitamin B, Vitamin C tăng cường miễn dịch và Vitamin E chống oxy hóa, giảm mệt mỏi, căng thẳng, cải thiện trí nhớ và giúp bạn minh mẫn hơn. Tăng cường sức đề kháng, hỗ trợ sức khỏe tổng thể'),
(4, 'NOW MK7 Vitamin K2 100 mcg',590000, 'Now', 'Sản phẩm giúp cải thiện sức khỏe xương, giảm nguy cơ loãng xương, ngăn ngừa bệnh tim mạch, hỗ trợ chống lại ung thư, tăng cường khả năng sử dụng năng lượng, và giảm hình thành sỏi thận, giảm viêm hiệu quả'),
(4, 'Nutricost Vitamin K2 MK-7 100 mcg',500000, 'Nutricost', 'Sản phẩm hỗ trợ tổng thể sức khỏe của xương, phòng tránh tình trạng loãng xương, gia tăng mật độ khoáng trong xương, bảo vệ sức khỏe tim mạch, và tăng cường trao đổi chất'),
(4, 'Bronson Vitamin D3 10,000 IU (250mcg)',280000, 'Bronson', 'Sản phẩm cung cấp 10.000 IU vitamin D3 mỗi viên, với hàm lượng cao, phù hợp cho người thường xuyên vận động hoặc kém hấp thụ vitamin D3. Nó giúp cải thiện sức khỏe xương, răng, cơ, dây chằng, giảm nguy cơ chấn thương, gãy xương, loãng xương, tăng cường hệ miễn dịch, hỗ trợ kiểm soát mức đường huyết và cải thiện mức testosterone.'),
(4, 'Applied Nutrition D3 3000IU, 90 Tablets',250000, 'Applied Nutrition', 'Sản phẩm giúp cải thiện hệ miễn dịch trong cơ thể, điều tiết calcium và phosphate, tác động trực tiếp trong việc sản xuất testosterone. Nó hỗ trợ bảo vệ sức khỏe xương, tổng hợp canxi trong cơ thể, cải thiện sức khỏe tim mạch, sản xuất serotonin để giảm căng thẳng và mệt mỏi, cải thiện trí nhớ não bộ và giúp kiểm soát lượng đường trong máu'),
(4, 'NOW Vitamin D-3 1000IU (25mcg), 90 Tablets',190000,'Now','1000IU/viên, tăng cường sức khỏe xương, răng, thúc đẩy hệ miễn dịch, sản xuất serotonin giảm căng thẳng, mệt mỏi, hỗ trợ cân bằng hormone, sức khỏe tim mạch, não bộ'),
(4, 'CodeAge Liposomal Vitamin C, 180 Capsules',1450000, 'Code Age', 'Sản phẩm cung cấp 1500mg Vitamin C và 15mg kẽm, chiết xuất từ trái cây, giúp giảm lão hóa, làm sáng da và tăng cường sức đề kháng. Màng bọc Liposome mang lại khả năng hấp thu vượt trội, không gây kích ứng hay khó chịu cho dạ dày'),
(4, 'Now Vitamin C-1000 Sustained Release with Rose Hips',400000, 'Now	', 'Sản phẩm giúp làm sáng da, mờ vết thâm mụn từ bên trong, tăng cường sức đề kháng, giảm mệt mỏi và hạn chế mắc các bệnh vặt như cảm cúm. Bổ sung dưỡng chất thiết yếu giúp cơ thể khỏe mạnh và dẻo dai mỗi ngày.'),
--Khoáng chất
(5, 'Nutricost Magnesium Bisglycinate Powder',750000, 'Nutricost', 'Sản phẩm chứa 210mg Magnesium từ 700mg Magnesium Glycinate, giúp hấp thụ tốt hơn và không gây đau bụng hay tiêu chảy. Dạng bột dễ pha uống, hỗ trợ giấc ngủ, kích thích xây dựng cơ bắp, tăng cường trí nhớ và sức khỏe xương. Ngoài ra, sản phẩm còn giúp hỗ trợ chứng đau nửa đầu và đau đầu'),
(5, 'Now Magnesium Caps 400 mg',450000, 'Now', 'Sản phẩm cung cấp 400mg nguyên tố Magie mỗi viên với hàm lượng cao và giá cả phải chăng. Giúp cải thiện tâm trạng, giảm stress, hỗ trợ giấc ngủ, giảm táo bón và khó tiêu, giảm chuột rút, đồng thời tăng cường sức khỏe xương khớp và tim mạch. Sản xuất tại cơ sở đạt chuẩn GMP.'),
(5, 'Ostrovit Liposomal Magnesium, 240 Capsules (60 Servings)',690000, 'Ostrovit', 'Sản phẩm chứa 400mg Magnesium, kết hợp giữa Magnesium Bisglycinate và Liposomal Magnesium, giúp tăng tốc độ hấp thụ và khả năng hấp thụ tốt hơn. Đưa Magiê đến đúng "đích", hạn chế tình trạng rối loạn tiêu hóa và duy trì nồng độ Magiê trong cơ thể, tránh bị đào thải'),
(5, 'Natural Factors Magnesium Bisglycinate, 200 mg (50 Servings) 120g',500000, 'Natural Factors', 'Sản phẩm cung cấp 200mg Magnesium Bisglycinate, hấp thụ nhanh hơn so với các dạng Magiê khác, không gây đau bụng hay tiêu chảy. Dạng bột tiện lợi, dễ pha uống, đạt chuẩn GMP. Hỗ trợ giấc ngủ, kích thích xây dựng cơ bắp, tăng cường trí nhớ và sức khỏe xương. Cũng hỗ trợ giảm đau nửa đầu và đau đầu'),
(5, 'Lake Avenue Magnesium Bisglycinate Chelate with TRAACS®, 100 mg',320000, 'Lake Avenue', 'Sản phẩm cung cấp 200mg Magnesium (từ 2.000mg Magnesium Bisglycinate) với khả năng hấp thu nhanh hơn các dạng Magiê khác và ít tác dụng phụ với đường tiêu hóa. Bổ sung khoáng chất Albion TRAACS® để tăng khả năng hấp thụ. Hỗ trợ giấc ngủ, kích thích xây dựng cơ bắp, tăng cường trí nhớ và sức khỏe xương, đồng thời hỗ trợ giảm đau nửa đầu và đau đầu. Đạt chuẩn GMP'),
(5, 'Tinh chất hàu Goodhealth Oyster Plus, 60 Capsules',249000, 'Goodhealth', 'Chiết xuất thịt hàu biển giúp tăng mức testosterone, cải thiện số lượng và chất lượng tinh trùng, hỗ trợ chức năng sinh sản cho cả nam và nữ. Ngoài ra, nó còn thúc đẩy phát triển cơ bắp, kiểm soát cân nặng và cải thiện sức khỏe tổng thể. Phù hợp cho cả nam và nữ'),
(5, 'Nutricost Zinc Picolinate Capsules - 30mg',490000, 'Nutricost', '30mg Zinc Picolinate có khả năng hấp thụ tốt hơn, hỗ trợ xây dựng cơ bắp, tăng khả năng sinh sản, cải thiện giấc ngủ và tăng cường chức năng miễn dịch'),
(5, 'NOW Zinc Glycinate 30 mg Albion TRAACS Zinc, 120 Softgels',350000, 'Now', '30mg Zinc (150mg Zinc Bisglycinate), 250mg dầu hạt bí ngô, hấp thu nhanh, duy trì lâu, không gây kích ứng dạ dày, hỗ trợ miễn dịch, sinh sản, sức khỏe da tóc, tim mạch.'),
(5, 'Natural Factors Zinc Bisglycinate 50 mg, 60 Vegetarian Capsules',480000, 'Natural Factors', 'Kẽm hấp thụ nhanh, không gây buồn nôn, táo bón, tăng cường miễn dịch, tốt cho da, móng, tóc, xương khớp, giảm căng thẳng, mệt mỏi, tăng cường sinh lý nam, thúc đẩy phục hồi cơ bắp'),
(5, 'OstroVit Zinc 60.000, 90 Tablets',250000, 'Ostrovit', '60mg Zinc Picolinate, hấp thụ tốt hơn, hỗ trợ xây dựng cơ bắp, tăng khả năng sinh sản, cải thiện giấc ngủ, tăng cường chức năng miễn dịch'),
(5, 'JYM Supplement Science ZMA JYM, 90 Vegetarian Capsules',650000, 'JYM', '30mg Kẽm, 450mg Magiê, 10,5mg Vitamin B6, 5mg BioPerine, tăng cường miễn dịch, thúc đẩy sức mạnh'),
(5, 'USN ZMA Z-MAG+, 180 Capsules',650000, 'USN', 'Kết hợp Kẽm, Magiê và Vitamin B6, tăng sản sinh hormone nam, cải thiện giấc ngủ, cải thiện tâm trạng, tăng cường hệ miễn dịch'),
(5, 'Blackmores Total Calcium Magnesium + D3, 200 Tablets',450000, 'Blackmores', 'Kết hợp 3 dạng canxi tăng khả năng hấp thụ, bổ sung Magiê + D3 chuyển hóa canxi hiệu quả, không gây kích ứng dạ dày, ngăn ngừa đào thải, duy trì nồng độ canxi, hỗ trợ xương khớp, cải thiện tim mạch, giảm stress, tăng thư giãn thần kinh'),
(5, 'NOW D.I.M 200 | Diindolylmethane with Calcium D-Glucarate, 90 Veg Capsules',500000, 'Now', '200mg D.I.M (Diindolylmethane), 100mg Calcium D-Glucarate, ổn định nội tiết tố, tăng cường chức năng sinh lý, hỗ trợ thải độc tốt hơn, tăng cường trí nhớ, cải thiện tâm trạng, hỗ trợ trái tim khỏe mạnh'),
(5, 'Now IRON 18 mg, 120 Veg Capsules',500000, 'Now', 'Cung cấp sắt tốt cho nhiều đối tượng, sắt dạng Bisglycinate gia tăng khả năng hấp thụ, Iron Bisglycinate 18mg, phòng ngừa thiếu máu, giảm thiểu mệt mỏi, hỗ trợ sản sinh năng lượng, tăng hiệu suất tập luyện, hỗ trợ thai kỳ khỏe mạnh'),
--Gia vị ăn kiêng
(6, 'Dầu xịt ăn kiêng - Cooking Spray PAM',300000, 'PAM', '0 Calo, 0 dính chảo, 0 Cholesterol, 0 Sodium, 0 Carbohydrate, 0 Protein'),
(6, 'Gia vị ăn kiêng McCormick Grill Mates, 70g',140000, 'McCormick', 'Gia vị ăn kiêng tẩm ướp. Low sodium.'),
(6, 'Gia vị ăn kiêng WEBER - 5.75oz (171g)',200000, 'Weber', 'Hàm lượng Sodium cực thấp, mùi vị đa dạng, tiện lợi sử dụng, không chứa bột ngọt và hóa chất bảo quản, không calo, chất béo, đường, carb, cholesterol, bảo quản dễ dàng, giá thành tiết kiệm với số lần dùng lớn'),
(6, 'Bơ đậu phộng Creamy Golden Farm, 340g',70000, 'Golden Farm', '100% từ đậu phộng củ chi, vị thơm ngon ngọt tự nhiên, tốt cho tim mạch, tăng cường sức khỏe toàn diện, béo ngọt tự nhiên, tăng cường sức đề kháng'),
(6, 'Dầu Xịt Ăn Kiêng Members Mark Olive Oil, 7oz (198g)',200000, 'Members Mark', '0 Calo, 0 Fat, 0 Sodium, 0 Carbohydrate, chống dính 100%, dễ dàng vệ sinh sau mỗi lần sử dụng. Số lần dùng có thể lên tới 700 lần xịt và sử dụng. Thích hợp với nhiều loại nồi, chảo khác nhau.'),
(6, 'Sốt ăn kiêng Fit Cuisine Low Calorie Sauce (425ml)',190000, 'Applied Nutrition', 'Hàm lượng calories thấp, không chứa đường, chất béo, không chứa gluten, thích hợp cho người ăn chay. Xuất xứ từ nước Anh'),
(6, 'Sốt ăn kiêng Ostrovit Sauce, 350 gram',210000, 'Ostrovit', '0 đường, 0 calo, 0 chất béo, đậm đà hương vị, thích hợp cho người ăn kiêng'),
(6, 'Bơ Đậu Phộng OstroVit Peanut Butter 100% Smooth, 1000 G',350000, 'Ostrovit', '100% làm từ đậu phộng, không muối, không đường, không chứa dầu cọ, cung cấp protein hỗ trợ phát triển cơ bắp.'),
(6, 'Dầu ăn dạng xịt - Spray Oil Crisco',200000, 'Crisco', 'Crisco là dầu ăn dạng xịt chống dính chảo - phù hợp cho chiên trứng, áp chảo gà, cá.'),
(6, 'Dầu ăn dạng xịt - Cooking Spray Great Value',220000, 'Great Value', 'Dầu ăn dạng xịt - Cooking Spray Great Value, không calo, không chất béo, dễ sử dụng, thích hợp cho nhiều loại nồi chảo, tiện lợi và dễ vệ sinh'),
--Phụ kiện thể thao
(7, 'Máy giãn cơ OstroVit Massage Gun OMG-01',1850000, 'Ostrovit', 'Máy mát xa với 30 chế độ hoạt động, 6 đầu mát xa đa năng, giúp giãn cơ, giảm căng cứng, đau nhức, tăng cường lưu thông máu, dễ dàng sử dụng'),
(7, 'HT Apparel Lever Belt - Đai lưng tập Gym Khóa Lẫy',1600000, 'HT APPAREL', 'Đai khóa Lever với cơ chế đóng/mở nhanh, độ dày 12mm, chất liệu Velvet/Satin, hỗ trợ vùng cơ trọng tâm, an toàn khi tập luyện. Màu sắc: Đỏ/Đen'),
(7, 'HT Apparel Leather Belt - Đai lưng tập Gym Khóa Cài - Pink Color',1199000, 'HT APPAREL', 'Đai lưng HT Apparel Leather Belt làm từ da bò cao cấp, thiết kế ôm trọn vùng cơ trọng tâm, hỗ trợ quá trình tập luyện, 10 nấc cài dễ dàng lựa chọn, độ bền cao, màu hồng'),
(7, 'Bình giữ nhiệt Revomax Inox 316L, 473 ml',699000, 'Gozen', 'Bình giữ nhiệt FDA chứng nhận, nắp thiết kế công nghệ Twist-Free với 3 điểm chạm, siêu chống tràn xoay 360 độ không rò rỉ. Giữ nhiệt 36 giờ lạnh, 18 giờ nóng, chống sốc nhiệt và va đập, van xả khí phù hợp đựng bia, nước có gas'),
(7, 'Găng tay tập Gym Harbinger',700000, 'Harbinger', 'Găng tay tập Gym chính hãng Harbinger'),
(7, 'Lót tạ - Harbinger Ergofit Bar Pad, 15 inch',700000, 'Harbinger', 'Thanh đỡ vai lót tạ đòn cao cấp Harbinger, nhập khẩu chính hãng. Hỗ trợ tập Squat không gây đau.'),
(7, 'Đệm vai gánh tạ - Harbinger Olympic Bar Pad 16 inch',500000, 'Harbinger', 'Đệm vai tập gym chính hãng Harbinger giúp bảo vệ cổ và vai khi gánh tạ'),
(7, 'Dụng cụ hỗ trợ tập cổ tay Fat Gripz, Extreme Size',450000, 'Fat Gripz', 'Dụng cụ hỗ trợ tập gia tăng sức mạnh cổ tay và sức nắm bàn tay. Thiết kế cao su, siêu bền'),
(7, 'Dây quấn cổ tay Harbinger Pro',460000, 'Harbinger', 'Dây quấn cổ tay hỗ trợ tập gym chính hãng Harbinger'),
(7, 'Cuốn bảo vệ đầu gối Aolikes Knee Wraps, 200 cm',350000, 'Aolikes', 'Chạy bộ, đạp xe, bóng đá, cầu lông, tennis, bóng bàn, bóng rổ, gym, yoga; chất liệu polyester, cao su, spandex; độ đàn hồi cao, phù hợp mọi kích thước chân; thiết kế bản rộng, dày hơn'),
(7, 'Con lăn tập bụng Gofit Onaroll Abdominal Wheel',349000, 'GoFit', 'GoFit Dual Exercise Ab Wheel giúp bạn đạt được kết quả mong muốn! Với khả năng tác động vào cả cơ bụng trên và bụng dưới, chiếc Ab Wheel này là công cụ lý tưởng để bạn có được cơ bụng săn chắc mà trước đây bạn chỉ có thể mơ ước'),
(7, 'Cân tiểu ly Unitech Kitchen Scal Model SU-3011',290000, 'Unitech', 'Cân tiểu ly điện tử chính xác đến từng gram, cân được từ 0.1g đến 5000g. Thiết kế nhỏ gọn, sang trọng với màu sắc tinh tế phù hợp với bếp hiện đại. Cảm ứng nhạy, thao tác dễ dàng. Cảnh báo quá tải và tự động ngắt sau 2 phút không sử dụng. Bảo hành 12 tháng'),
(7, 'Bình nước Applied Nutrition 1000ml',200000, 'Applied Nutrition', 'Dung tích 1000ml,nhựa BPA-Free, chất lượng cao, dễ dàng cầm nắm, mang đi'),
(7, 'Dây kéo lưng - Gymstore.vn Premium Lifting Padded Lifting Straps',160000, 'Gymstore', 'Đệm cổ tay có thể điều chỉnh kích thước, chất liệu vải dày, cao cấp, độ bền cao và độ bám tốt. Cố định cổ tay, tăng sức kéo và sức nắm, bảo vệ cổ tay, khớp ngón tay và lòng bàn tay. Sản phẩm độc quyền được sản xuất và thiết kế bởi Gymstore'),
(7, 'Vợt Pickleball Joola Ben Johns Hyperion C2 CFS 14',5500000, 'Joola', 'Trọng lượng nhẹ 221g, bề mặt vợt làm từ sợi carbon-flex5, độ dày 14mm cho lực bật tốt, tay cầm thoải mái, cân bằng sức mạnh và khả năng kiểm soát. Được USAPA chấp thuận để chơi trong giải đấu.');




-- Chèn dữ liệu vào bảng Orders
INSERT INTO Orders (customer_id, total_price,address,phone_number,status)
VALUES 
(1, 500000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(2, 2000000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(3, 300000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(4, 1500000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(5, 250000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(6, 10000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(7, 8000000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(8, 1000000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(9, 35000,'123 Hai ba trung, Ha noi','0123456789', 'Completed'),
(10, 4000000,'123 Hai ba trung, Ha noi','0123456789', 'Completed');

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

--Trigger dùng để update lại thông tin tổng tiền sản phẩm trong đơn hàng mỗi khi thêm hay xóa thông tin OrderDetail tương ứng 
CREATE OR REPLACE FUNCTION update_order_total_price()
RETURNS TRIGGER AS $$
DECLARE
    price_amount_del DECIMAL(10,2);
	price_amount_ins DECIMAL(10,2);
BEGIN
    IF (TG_OP = 'INSERT') THEN
        SELECT p.product_price * NEW.product_amount INTO price_amount_ins FROM Product p
		WHERE p.product_id = NEW.product_id;

        UPDATE Orders
        SET total_price = total_price + price_amount_ins 
        WHERE orders_id = NEW.orders_id;

	ELSIF (TG_OP = 'UPDATE') THEN
        SELECT p.product_price * NEW.product_amount INTO price_amount_ins FROM Product p
		WHERE p.product_id = NEW.product_id;

		SELECT p.product_price * OLD.product_amount INTO price_amount_del FROM Product p
		WHERE p.product_id = OLD.product_id;

        UPDATE Orders
        SET total_price = total_price + price_amount_ins - price_amount_del
        WHERE orders_id = NEW.orders_id;

    ELSIF (TG_OP = 'DELETE') THEN
        SELECT p.product_price * OLD.product_amount INTO price_amount_del FROM Product p
		WHERE p.product_id = OLD.product_id;

        UPDATE Orders
        SET total_price = total_price - price_amount_del
        WHERE orders_id = OLD.orders_id;

    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

--Trigger cập nhật lại order_date trên order khi thêm mới một Payment 
CREATE OR REPLACE FUNCTION update_order_date()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE Orders
    SET order_date = NEW.payment_date
    WHERE order_id = NEW.order_id;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

--Kích hoạt trigger
DROP TRIGGER IF EXISTS trg_generate_workout_details ON PlanDetail;
DROP TRIGGER IF EXISTS trg_update_calories_remain ON FoodDetail;
DROP TRIGGER IF EXISTS trg_update_daily_weight ON Customer_Health;
DROP TRIGGER IF EXISTS trg_update_order_total_price ON OrderDetail;

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

CREATE TRIGGER trg_update_order_total_price
AFTER INSERT OR DELETE OR UPDATE ON OrderDetail
FOR EACH ROW
EXECUTE FUNCTION update_order_total_price();

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


-- --Test User, chỉnh lại ngày ở phần insert into dailydiary nếu cần sử dụng
-- INSERT INTO Customer (customer_name, auth_type, username, customer_password, role_user, email)
-- VALUES 
-- ('Tran Van Test', 1, 'test', 'test', 2,'hminhquan111@gmail.com');

-- INSERT INTO Customer_Health (customer_id, gender, height, weight, age, activity_level, body_fat, tdee)
-- VALUES 
-- (11, 1, 180, 90, 21, 1.55, 20.5, 2984);

-- INSERT INTO DailyDiary (customer_id, diary_date,calories_remain, daily_weight, total_calories, notes)
-- VALUES 
-- (11, '2024-11-30', 2984, 88, 2984, 'Feeling great today'),
-- (11, '2024-12-01', 2984, 89, 2984, 'Feeling great today'),
-- (11, '2024-12-02', 2984, 89, 2984, 'Feeling great today'),
-- (11, '2024-12-03', 2984, 87, 2984, 'Feeling great today'),
-- (11, '2024-12-04', 2984, 88, 2984, 'Feeling great today'),
-- (11, '2024-12-05', 2984, 89, 2984, 'Feeling great today'),
-- (11, '2024-12-06', 2984, 91, 2984, 'Feeling great today'),
-- (11, '2024-12-07', 2984, 90, 2984, 'Feeling great today');

-- --30/11
-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (11, 1, 1, 1),  -- Chicken Breast, 1 serving = 165 calories
-- (11, 2, 1, 2),  -- Rice, 2 servings = 260 calories
-- (11, 6, 1, 2),  -- Eggs, 2 servings = 310 calories
-- (11, 9, 1, 2),  -- Milk, 200ml = 84 calories
-- (11, 5, 2, 1),  -- Salmon, 1 serving = 208 calories
-- (11, 7, 2, 2),  -- Oatmeal, 2 servings = 778 calories
-- (11, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (11, 4, 3, 1),  -- Apple, 1 quả = 52 calories
-- (11, 10, 3, 1), -- Pasta, 1 serving = 131 calories
-- (11, 8, 3, 2),  -- Almonds, 2 servings = 1,158 calories
-- (11, 2, 4, 1),  -- Rice, 1 serving = 130 calories
-- (11, 6, 4, 1),  -- Eggs, 1 serving = 155 calories
-- (11, 3, 4, 1);  -- Broccoli, 100g = 55 calories


-- -- Dữ liệu cho DailyDiary ID = 12 (Tổng calories = khoảng 2850)

-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (12, 1, 1, 2),  -- Chicken Breast, 2 servings = 330 calories
-- (12, 2, 1, 2),  -- Rice, 2 servings = 260 calories
-- (12, 6, 1, 2),  -- Eggs, 2 servings = 310 calories
-- (12, 9, 1, 2),  -- Milk, 200ml = 84 calories
-- (12, 5, 2, 1),  -- Salmon, 1 serving = 208 calories
-- (12, 7, 2, 2),  -- Oatmeal, 2 servings = 778 calories
-- (12, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (12, 4, 3, 1),  -- Apple, 1 quả = 52 calories
-- (12, 10, 3, 1), -- Pasta, 1 serving = 131 calories
-- (12, 8, 3, 1),  -- Almonds, 1 serving = 579 calories
-- (12, 2, 4, 1),  -- Rice, 1 serving = 130 calories
-- (12, 6, 4, 1),  -- Eggs, 1 serving = 155 calories
-- (12, 3, 4, 1);  -- Broccoli, 100g = 55 calories

-- -- Dữ liệu cho DailyDiary ID = 13 (Tổng calories = khoảng 2800)
-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (13, 1, 1, 3),  -- Chicken Breast, 3 servings = 495 calories
-- (13, 2, 1, 2),  -- Rice, 2 servings = 260 calories
-- (13, 6, 1, 2),  -- Eggs, 2 servings = 310 calories
-- (13, 9, 1, 2),  -- Milk, 200ml = 84 calories
-- (13, 5, 2, 1),  -- Salmon, 1 serving = 208 calories
-- (13, 7, 2, 2),  -- Oatmeal, 2 servings = 778 calories
-- (13, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (13, 4, 3, 1),  -- Apple, 1 quả = 52 calories
-- (13, 10, 3, 1), -- Pasta, 1 serving = 131 calories
-- (13, 8, 3, 1),  -- Almonds, 1 serving = 579 calories
-- (13, 2, 4, 1),  -- Rice, 1 serving = 130 calories
-- (13, 6, 4, 1),  -- Eggs, 1 serving = 155 calories
-- (13, 3, 4, 1);  -- Broccoli, 100g = 55 calories

-- -- Dữ liệu cho DailyDiary ID = 14 (Tổng calories ~ 2500)

-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (14, 1, 1, 2),  -- Chicken Breast, 2 servings = 330 calories
-- (14, 2, 1, 2),  -- Rice, 2 servings = 260 calories
-- (14, 9, 1, 2),  -- Milk, 200ml = 84 calories
-- (14, 5, 2, 1),  -- Salmon, 1 serving = 208 calories
-- (14, 6, 2, 1),  -- Eggs, 1 serving = 155 calories
-- (14, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (14, 7, 3, 3),  -- Oatmeal, 3 servings = 1167 calories
-- (14, 4, 4, 1);  -- Apple, 1 quả = 52 calories

-- -- Dữ liệu cho DailyDiary ID = 14 (Tổng calories ~ 2500)

-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (15, 1, 1, 3),  -- Chicken Breast, 3 servings = 495 calories
-- (15, 2, 1, 2),  -- Rice, 2 servings = 260 calories
-- (15, 5, 2, 1),  -- Salmon, 1 serving = 208 calories
-- (15, 6, 2, 2),  -- Eggs, 2 servings = 310 calories
-- (15, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (15, 7, 3, 2),  -- Oatmeal, 2 servings = 778 calories
-- (15, 4, 4, 1);  -- Apple, 1 quả = 52 calories

-- -- Dữ liệu cho DailyDiary ID = 14 (Tổng calories ~ 2500)

-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (16, 1, 1, 2),  -- Chicken Breast, 2 servings = 330 calories
-- (16, 2, 1, 2),  -- Rice, 2 servings = 260 calories
-- (16, 9, 1, 1),  -- Milk, 100ml = 42 calories
-- (16, 5, 2, 2),  -- Salmon, 2 servings = 416 calories
-- (16, 6, 2, 1),  -- Eggs, 1 serving = 155 calories
-- (16, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (16, 7, 3, 2),  -- Oatmeal, 2 servings = 778 calories
-- (16, 10, 4, 1);  -- Pasta, 1 serving = 131 calories

-- -- Dữ liệu cho DailyDiary ID = 14 (Tổng calories ~ 2500)

-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (17, 1, 1, 3),  -- Chicken Breast, 3 servings = 495 calories
-- (17, 2, 1, 1),  -- Rice, 1 serving = 130 calories
-- (17, 9, 1, 1),  -- Milk, 100ml = 42 calories
-- (17, 6, 2, 2),  -- Eggs, 2 servings = 310 calories
-- (17, 5, 2, 1),  -- Salmon, 1 serving = 208 calories
-- (17, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (17, 7, 3, 2),  -- Oatmeal, 2 servings = 778 calories
-- (17, 4, 4, 1);  -- Apple, 1 quả = 52 calories

-- -- Dữ liệu cho DailyDiary ID = 18 (Tổng calories ~ 2500)

-- INSERT INTO FoodDetail (dailydiary_id, food_id, meal_type, food_amount) VALUES
-- (18, 1, 1, 2),  -- Chicken Breast, 2 servings = 330 calories
-- (18, 2, 1, 2),  -- Rice, 2 servings = 260 calories
-- (18, 9, 1, 1),  -- Milk, 100ml = 42 calories
-- (18, 5, 2, 1),  -- Salmon, 1 serving = 208 calories
-- (18, 6, 2, 2),  -- Eggs, 2 servings = 310 calories
-- (18, 3, 2, 1),  -- Broccoli, 100g = 55 calories
-- (18, 7, 3, 2),  -- Oatmeal, 2 servings = 778 calories
-- (18, 4, 4, 1);  -- Apple, 1 quả = 52 calories

-- INSERT INTO PlanDetail (plan_id, customer_id, start_date, end_date)
-- VALUES (2, 11, '2024-11-30', '2025-01-25');

-- UPDATE WorkoutDetail
-- SET description = 'Đã hoàn thành Exercise trong ngày'
-- WHERE workoutdetail_id = 1;

-- UPDATE WorkoutDetail
-- SET description = 'Đã hoàn thành Exercise trong ngày'
-- WHERE workoutdetail_id = 2;

-- UPDATE WorkoutDetail
-- SET description = 'Đã hoàn thành Exercise trong ngày'
-- WHERE workoutdetail_id = 3;

-- UPDATE WorkoutDetail
-- SET description = ''
-- WHERE workoutdetail_id = 4;

select * from orders;
select * from payment;
select * from orderdetail
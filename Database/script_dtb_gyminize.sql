DROP TABLE IF EXISTS OrderDetail CASCADE;
DROP TABLE IF EXISTS Orders CASCADE;
DROP TABLE IF EXISTS Product CASCADE;
DROP TABLE IF EXISTS Category CASCADE;
DROP TABLE IF EXISTS FoodDetail CASCADE;
DROP TABLE IF EXISTS Food CASCADE;
DROP TABLE IF EXISTS DailyDiary CASCADE;
DROP TABLE IF EXISTS ExerciseDetail CASCADE;
DROP TABLE IF EXISTS Exercise CASCADE;
DROP TABLE IF EXISTS WorkoutDetail CASCADE;
DROP TABLE IF EXISTS WorkoutDay CASCADE;
DROP TABLE IF EXISTS PlanDetail CASCADE;
DROP TABLE IF EXISTS Plan CASCADE;
DROP TABLE IF EXISTS Customer_Health CASCADE;
DROP TABLE IF EXISTS Customer CASCADE;




CREATE TABLE IF NOT EXISTS Customer (
	customer_id SERIAL PRIMARY KEY,
	customer_name VARCHAR(100) ,
    auth_type INT,  
    username VARCHAR(100), -- thông username/password ( có thể null nếu dùng email)         
    customer_password VARCHAR(255),                 
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE Customer_Health
ALTER COLUMN tdee TYPE NUMERIC(10, 2);
CREATE TABLE IF NOT EXISTS Customer_Health (
    customer_id INT PRIMARY KEY, 
	gender INT ,
    height INT,                                          
    weight INT,                                          
    age INT,                                             
    activity_level INT,                                  
    body_fat NUMERIC(10, 2),                              
    tdee NUMERIC(10, 2),                                  
    FOREIGN KEY (customer_id) REFERENCES Customer(customer_id) 
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
CREATE TABLE IF NOT EXISTS WorkoutDay (
    workoutday_id SERIAL PRIMARY KEY,
    workoutday_type VARCHAR(100), --nhóm cơ tập đến
	workoutday_order INT NOT NULL, --thứ tự ngày tập (2->8)
	description TEXT
);

-- Tạo bảng WorkoutDetail
CREATE TABLE IF NOT EXISTS WorkoutDetail (
    workoutdetail_id SERIAL PRIMARY KEY,
    workoutday_id INT REFERENCES WorkoutDay(workoutday_id),
	plan_id INT REFERENCES Plan(plan_id),
    description TEXT
);

-- Tạo bảng Exercise
CREATE TABLE IF NOT EXISTS Exercise (
    exercise_id SERIAL PRIMARY KEY,
    exercise_name VARCHAR(255) NOT NULL,
    description TEXT
);

-- Tạo bảng ExerciseDetail
CREATE TABLE IF NOT EXISTS ExerciseDetail (
    exercisedetail_id SERIAL PRIMARY KEY,
    workoutdetail_id INT REFERENCES WorkoutDetail(workoutdetail_id),
    exercise_id INT REFERENCES Exercise(exercise_id),
    repetitions INT,
    workout_sets INT
);

-- Tạo bảng DailyDiary
CREATE TABLE IF NOT EXISTS DailyDiary (
    dailydiary_id SERIAL PRIMARY KEY,
    customer_id INT REFERENCES Customer(customer_id),
    diary_date DATE NOT NULL,
	calories_remain INT NOT NULL,
    notes TEXT
);

-- Tạo bảng Food
CREATE TABLE IF NOT EXISTS Food (
    food_id SERIAL PRIMARY KEY,
    food_name VARCHAR(255) NOT NULL,
    calories INT,  -- lượng calo tính trên mỗi khẩu phần ăn
    protein FLOAT,
    carbs FLOAT,
    fats FLOAT
);

-- Tạo bảng FoodDetail
CREATE TABLE IF NOT EXISTS FoodDetail (
    fooddetail_id SERIAL PRIMARY KEY,
    dailydiary_id INT REFERENCES DailyDiary(dailydiary_id),
    food_id INT REFERENCES Food(food_id),
    food_amount INT  -- kích cỡ khẩu phần ăn (gram)
);

-- Tạo bảng Category
CREATE TABLE IF NOT EXISTS Category (
    category_id SERIAL PRIMARY KEY,
    categoty_name VARCHAR(255) NOT NULL,
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

-- Tạo bảng Order
CREATE TABLE IF NOT EXISTS Orders(
    orders_id SERIAL PRIMARY KEY,
    customer_id INT REFERENCES Customer(customer_id),
    order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    total_price DECIMAL(10, 2)
);

-- Tạo bảng OrderDetail
CREATE TABLE IF NOT EXISTS OrderDetail (
    orderdetail_id SERIAL PRIMARY KEY,
    product_id INT REFERENCES Product(product_id),
    orders_id INT REFERENCES Orders(orders_id),
	product_amount INT NOT NULL
);




ALTER SEQUENCE customer_customer_id_seq RESTART WITH 1;
ALTER SEQUENCE plan_plan_id_seq RESTART WITH 1;
ALTER SEQUENCE plandetail_plandetail_id_seq RESTART WITH 1;
ALTER SEQUENCE workoutday_workoutday_id_seq RESTART WITH 1;
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

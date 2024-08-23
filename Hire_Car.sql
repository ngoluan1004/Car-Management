CREATE DATABASE Hire_Car

CREATE TABLE tblRole
(
	iRoleID int IDENTITY,
	sRoleName nvarchar(50),
	CONSTRAINT PK_tblRole PRIMARY KEY (iRoleID),
)
INSERT INTO tblRole (sRoleName)
VALUES ('Admin'), ('Customer');

select*from tblRole

CREATE TABLE tblUsers	 (
    user_id INT IDENTITY(1,1) PRIMARY KEY, -- ID người dùng, tự tăng
    username NVARCHAR(50) NOT NULL,         -- Tên người dùng
    email VARCHAR(100) NOT NULL,           -- Địa chỉ email
    password NVARCHAR(100) NOT NULL,        -- Mật khẩu người dùng
    full_name NVARCHAR(100) NOT NULL,       -- Họ và tên người dùng
    phone_number VARCHAR(20),              -- Số điện thoại người dùng
    address VARCHAR(255),                  -- Địa chỉ người dùng
    date_of_birth DATE,                    -- Ngày sinh
	iUserRoleID INT
	CONSTRAINT FK_tblUser_tblRole FOREIGN KEY (iUserRoleID) REFERENCES tblRole(iRoleID)
);

INSERT INTO tblUSERS (username, email, password, full_name, phone_number, address, date_of_birth, iUserRoleID)
VALUES ('admin', 'user1@example.com', 'admin123', 'User One', '1234567890', '123 Main St', '1990-01-01', 1);

INSERT INTO tblUSERS (username, email, password, full_name, phone_number, address, date_of_birth, iUserRoleID)
VALUES ('user2', 'user2@example.com', 'password2', 'User Two', '0987654321', '456 Elm St', '1992-02-02', 2);

INSERT INTO tblUSERS (username, email, password, full_name, phone_number, address, date_of_birth, iUserRoleID)
VALUES ('kynguyen', 'kynguyen@example.com', 'kynguyen', N'Nguyễn Bá Kỳ', '0987654321', '456 Elm St', '1992-02-02', 2);

INSERT INTO tblUSERS (username, email, password, full_name, phone_number, address, date_of_birth, iUserRoleID)
VALUES ('luanngo', 'luanngo@example.com', 'luanngo', N'Ngô Văn Luân', '0987654321', 'Truong Chinh', '2000-10-24', 2);

select*from tblUsers
delete from tblUsers
drop table tblUsers

CREATE TABLE tblCars	 (
    car_id INT IDENTITY(1,1) PRIMARY KEY,
    car_make NVARCHAR(50) NOT NULL,         -- Hãng sản xuất 
    car_model nVARCHAR(100) NOT NULL,       -- Mẫu xe
    year_production INT NOT NULL,			-- Năm sản xuất
    color NVARCHAR(100) NOT NULL,			-- Màu xe
    price_per_day FLOAT(20) NOT NULL,     -- Giá thuê trên ngày
    image_url VARCHAR(255) NOT NULL,        -- Đường dẫn hình ảnh của xe
    rating DECIMAL(3,2),					-- Điểm đánh giá 
	license_plate NVARCHAR(10) NOT NULL,	-- Biển số xe
	seats INT NOT NULL,						-- Số ghế của xe
	transmission NVARCHAR(10) NOT NULL,		-- Loại hộp số
	fuel_type NVARCHAR(20)					-- Loại nhiên liệu
);

select*from tblCars
delete from tblCars
drop table tblCars
drop table tblBooking
INSERT INTO tblCars 
(car_make, car_model, year_production, color, price_per_day, image_url, rating, license_plate, seats, transmission, fuel_type) 
VALUES 
('Mercedes', 'MayBach', 2020, N'Đỏ', 50, 'https://mercedes-vietnam.com.vn/wp-content/uploads/2021/01/Mercedes-Maybach-S680-2022-mercedes-vietnam-1.jpg', 3.2, 'AB1234', 5, N'Tự động', N'Xăng'),
('Mercedes', 'Benz GLC', 2019, N'Trắng', 45, 'https://mercedes-vietnam.com.vn/wp-content/uploads/2021/01/Mercedes-Maybach-S680-2022-mercedes-vietnam-1.jpg', 3.0, 'CD5678', 5, N'Tự động', N'Xăng'),
('Mercedes', 'Benz S-Class', 2018, N'Đen', 40, 'https://mercedes-vietnam.com.vn/wp-content/uploads/2021/01/Mercedes-Maybach-S680-2022-mercedes-vietnam-1.jpg', 3.9, 'EF9012', 5, N'Số tay', N'Dầu');


select*from tblUser


CREATE TABLE tblBooking	 (
    booking_id INT IDENTITY(1,1) PRIMARY KEY,	-- Mã đặt xe
    user_booking_id INT NOT NULL,				-- ID người dùng 
    car_booking_id INT NOT NULL,				-- ID ô tô
    booking_date DATE NOT NULL,					-- Ngày đặt xe
    rental_start_date DATE NOT NULL,			-- Ngày bắt đầu thuê
    rental_end_date DATE NOT NULL,              -- Ngày kết thúc thuê
    total_cost Decimal(10,2) NOT NULL,			-- Tổng chi phí giao dịch thuê xe
    status_car NVARCHAR(20)	NOT NULL			-- Trạng thái của đặt xe
	CONSTRAINT FK_tblUsers_tblBooking FOREIGN KEY (user_booking_id) REFERENCES tblUsers(user_id),
	CONSTRAINT FK_tblCar_tblBooking FOREIGN KEY (car_booking_id) REFERENCES tblCars(car_id)
);


drop table tblBooking
select*from tblBooking

create table tblComment (
	comment_id int  IDENTITY(1,1) primary key,
	user_id int not null, 
	car_id int not null,
	content nvarchar(1000),
	verifyKey nvarchar(30) not null,
	CONSTRAINT FK_tblCmt_tblCar FOREIGN KEY (car_id) REFERENCES tblCars(car_id),
	CONSTRAINT FK_tblCmt_tblUser FOREIGN KEY (user_id) REFERENCES tblUsers(user_id)
) drop table tblComment

INSERT INTO tblComment (user_id, car_id, content,verifyKey)
VALUES
    (2, 5, 'xe rat dep chay rat la muot ma','1adadadas'),
    (3, 6, 'gia ca hop li , lam viec nhanh chong','1adaddadadas'),
    (4, 3, 'dich vu tot, xe cung moi','1adadadascc'),
	(2, 4, 'toi rat thich','1adadadasffs'),
    (3, 5, 'xe rat đep ','1adadadasff'),
    (4, 6, 'can phai bao duong xe tot hon','1adadadasbb'),
	(2, 3, 'toi se ung ho dai dai','1adadadasêe'),
    (3, 4, 'xe chay em, buong lai sach se','1adadadas'),
    (4, 5, 'toi rat thich cach phuc vu o đay','1adadadasqqww'),
	(2, 6, 'xe rat đep chay rat la muot ma','1adadadasdd'),
    (3, 3, 'gia ca hop li , lam viec nhanh chong','1adadadaseqeqe'),
    (4, 4, 'dich vu tot, xe cung moi','1adeqeqadadas');
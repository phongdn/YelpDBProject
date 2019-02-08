CREATE TABLE Yelp_User (
	user_id	CHAR(24),
	review_count INTEGER,
	user_name VARCHAR(30),
	user_zipcode INTEGER,
	birth_date CHAR(10),
	friends_count INTEGER,
	user_phone_number INTEGER,
	PRIMARY KEY (user_id)
)

CREATE TABLE Check_In (
	times CHAR(24),
	PRIMARY KEY (times)
)

CREATE TABLE Creates (
	times CHAR(24),
	user_id CHAR(24),
	PRIMARY KEY (user_id, times),
	FOREIGN KEY(user_id) REFERENCES Yelp_User(user_id),
	FOREIGN KEY(times) REFERENCES Check_In(times)
)

CREATE TABLE Business (
	business_name varchar(30),
	stars INTEGER,
	business_id INTEGER,
	location char(30),
	is_open char(10),
	hours char(80),
	PRIMARY KEY(business_id)
)

CREATE TABLE Located_At (
	times char(30),
	business_id INTEGER,
	PRIMARY KEY(business_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id),
	FOREIGN KEY(times) REFERENCES Check_In(times)
)

CREATE TABLE Reviews (
	review_description CHAR(200),
	review_id INTEGER,
	review_date CHAR(20),
	votes INTEGER,
	stars INTEGER,
	PRIMARY KEY(review_id)
)

CREATE TABLE Belongs_To (
	business_id INTEGER,
	review_id INTEGER,
	PRIMARY KEY(review_id, business_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id),
	FOREIGN KEY(review_id) REFERENCES Reviews(review_id)
)

CREATE TABLE Writes (
	review_id INTEGER;
	user_id INTEGER;
	PRIMARY KEY(review_id, user_id),
	FOREIGN KEY(review_id) REFERENCES Reviews(review_id),
	FOREIGN KEY(user_id) REFERENCES Yelp_User(user_id)
)

CREATE TABLE Has (
	business_id INTEGER,
	user_id INTEGER,
	PRIMARY KEY(business_id, user_id),
	FOREIGN KEY(user_id) REFERENCES Yelp_User(user_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
)

CREATE TABLE Categories (
	category_id INTEGER,
	PRIMARY KEY(category_id)
)

CREATE TABLE Type_Of (
	business_id INTEGER,
	category_id INTEGER,
	PRIMARY KEY(category_id),
	FOREIGN KEY(category_id) REFERENCES Categories(category_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
)
CREATE TABLE Restaurants (
	category_id INTEGER,
	PRIMARY KEY(category_id),
	FOREIGN KEY(category_id) REFERENCES Categories(category_id)
)

CREATE TABLE Home_Services (
	category_id INTEGER,
	PRIMARY KEY(category_id),
	FOREIGN KEY(category_id) REFERENCES Categories(category_id)
)

CREATE TABLE Auto_Services (
	category_id INTEGER,
	PRIMARY KEY(category_id),
	FOREIGN KEY(category_id) REFERENCES Categories(category_id)
)

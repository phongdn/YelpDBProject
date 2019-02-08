CREATE TABLE Yelp_User (
	user_id	CHAR(24),
	review_count INTEGER,
	name VARCHAR(30),
	zipcode INTEGER,
	birth_date CHAR(10),
	friends_count INTEGER,
	phone_number INTEGER,
	PRIMARY KEY (user_id)
)

CREATE TABLE Check_In (
	times char(24),
	PRIMARY KEY (times)
)

CREATE TABLE Creates (
	times char(24),
	user_id char(24),
	PRIMARY KEY (user_id, times),
	FOREIGN KEY(user_id) REFERENCES Yelp_User(user_id),
	FOREIGN KEY(times) REFERENCES Check_In(times)
)

CREATE TABLE Business (
	name varchar(30),
	stars INTEGER,
	business_id INTEGER,
	location 
)
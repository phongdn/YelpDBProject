CREATE TABLE Users (
	user_id		CHAR(22) PRIMARY KEY,
	average_stars	FLOAT CHECK (average_stars >= 0 AND average_stars <= 5), -- rating from 0 to 5 stars (1/2 stars allowed) --
	cool	INTEGER,
	funny	INTEGER,
	useful	INTEGER,
	user_name	VARCHAR(50) NOT NULL,
	fans	INTEGER,
	review_count	INTEGER,
	yelping_since	DATE NOT NULL 
);

CREATE TABLE Business (
	business_id		CHAR(22) PRIMARY KEY,
	business_name		VARCHAR(100) NOT NULL,
	address		VARCHAR(100) NOT NULL,
	city		VARCHAR(25) NOT NULL,
	statecode		CHAR(2) NOT NULL,
	postalcode		CHAR(5) NOT NULL,
	business_latitude		FLOAT NOT NULL,
	business_longitude		FLOAT NOT NULL,
	business_review_count		INTEGER NOT NULL,
	is_open		BOOLEAN NOT NULL, --INTEGER CHECK (is_open = 0 OR is_open = 1) NOT NULL, -- 0 for false and 1 for true --
	num_checkins		INTEGER NOT NULL,
	review_rating		FLOAT NOT NULL,
	business_stars		FLOAT CHECK (business_stars >= 0 AND business_stars <= 5) NOT NULL -- rating from 0 to 5 stars (1/2 stars allowed) --
);

CREATE TABLE Attributes (
	-- Two person team, therefore we don't need to include business atrributes --
	attr_name	VARCHAR(25) ,
	attr_value		INTEGER,
	business_id CHAR(22),
	PRIMARY KEY(attr_name, business_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Categories (
	category_name		VARCHAR(50),
	business_id		CHAR(22),
	PRIMARY KEY(category_name, business_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Hours (
	week_day	CHAR(10),
	closes	TIME,
	opens	TIME,
	business_id		CHAR(22),
	PRIMARY KEY(week_day, business_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Checkins (
	checkin_day		VARCHAR(10),
	counts		INTEGER, --total checkins that day
	before_noon		INTEGER NOT NULL,
	after_noon		INTEGER NOT NULL,
	--times		TIME NOT NULL,  --We can't just use each individual otherwise insert will take hours, thus we simplify it into mornings and evenings
	business_id		CHAR(22),
	PRIMARY KEY(checkin_day, business_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Review (
	review_id	CHAR(22) PRIMARY KEY,
	user_id		CHAR(22) NOT NULL,
	business_id		CHAR(22) NOT NULL,
	review_stars	FLOAT CHECK(review_stars >= 0 AND review_stars <= 5) NOT NULL, -- rating from 0 to 5 stars (1/2 stars allowed) --
	dates	DATE NOT NULL,
	text	VARCHAR(3000) NOT NULL,
	useful_vote		INTEGER,
	funny_vote		INTEGER,
	cool_vote		INTEGER,
	FOREIGN KEY(user_id) REFERENCES Users(user_id),
	FOREIGN KEY(business_id) REFERENCES Business(business_id)
);

CREATE TABLE Friends (
	friend_id	CHAR(22),
	user_id		CHAR(22),
	PRIMARY KEY(user_id, friend_id),
	FOREIGN KEY(user_id) REFERENCES Users(user_id),
	FOREIGN KEY(friend_id) REFERENCES Users(user_id)
);



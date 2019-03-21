import json
import psycopg2

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            sql_str = "INSERT INTO Business (business_id, business_name, address,statecode,city,postalcode,business_latitude,business_longitude,business_stars,business_review_count,is_open,num_checkins,review_rating) " + \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) + "','" + \
                      cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["postal_code"]) + "'," + str(data["latitude"]) + "," + \
                      str(data["longitude"]) + "," + str(data["stars"]) + "," + str(data["review_count"]) + "," + int2BoolStr(data["is_open"]) + ",0"  + ",0 "  + ");" 
                      #int2BoolStr(data["is_open"])
                       
            try:
                cur.execute(sql_str)
            except psycopg2.Error as e:
                print("Insert to businessTABLE failed!")
                print("Error Message: ", e) 
                break  #if we get a single failure or error, end the loop
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            #outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()
    print("business list: ")
    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

#   From milestone1:
#
# def parseObject(outfile, data):
#     for key, value in data.items():
#         #outfile.write(cleanStr4SQL(key)+'\t')
#         if (isinstance(value, dict)):
#             parseObject(outfile, value)
#         else:
#             #outfile.write(str(value)+'\t')

def parseHours(h, bus_id): #Creates the list of SQL commands (hours, businessID)
    returnListOfSQL = [] #initialize list of SQL commands to return
    for k,v in h.items(): #foreach day M-F in the business hours. hours.items = (key,value) 
        #print("K: " + k + " V: " + v)
        # e.g) v = 9:00-12:00
        #      k = Monday
        temp = v.split("-", 1) #(separator, max) 
        #temp[0] = open
        #temp[1] = close
        tempSQLCommand = "INSERT INTO Hours (week_day, closes, opens, business_id) " + \
                         "VALUES ('" + str(k) + "','" + temp[1] + "','" + temp[0] + "','" + bus_id + "');"
        returnListOfSQL.append(tempSQLCommand)
    return returnListOfSQL




def insert2HoursTable():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    
        #outfile =  open('./yelp_hours.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0
        total_hours_added = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # We'll need to parse hours object for each line since its inside the business object
            hoursSQL = parseHours(data['hours'], cleanStr4SQL(data['business_id'])) #grab the hours object and business_id at current line

            # Generate the INSERT statement for the current business hours
            # include values for all hoursTable attributes
            for item in hoursSQL:
                sql_str = item
                try:
                    cur.execute(sql_str)
                except psycopg2.Error as e:
                    print("Insert to hoursTABLE failed!")
                    print("Error Message: ", e)
                    return #if we get a single failure or error, end the loop
                conn.commit()
                # optionally you might write the INSERT statement to a file.
                #outfile.write(sql_str)
                total_hours_added += 1

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()
    print("hours list: ")
    print(count_line)
    print(total_hours_added)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


#Dont need to add business attributes data to server because we are a two person team


def insert2CategoriesTable():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    
        #outfile =  open('./yelp_categories.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0
        total_categories_added = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # include values for all categoriesTable attributes

            # e.g) Categroies: ["Sandwiches", "Restaurants"]
            # So we'll only need to loop through the category list per line
            for item in data['categories']:
                sql_str = "INSERT INTO Categories (category_name, business_id) " + \
                        "VALUES ('" + cleanStr4SQL(item) + "','" + data['business_id'] + "');"
                        #int2BoolStr(data["is_open"])
                        
                try:
                    cur.execute(sql_str)
                except psycopg2.Error as e:
                    print("Insert to categoriesTABLE failed!")
                    print("Error Message: ", e) 
                    return  #if we get a single failure or error, end the loop
                conn.commit()
                # optionally you might write the INSERT statement to a file.
                #outfile.write(sql_str)
                total_categories_added += 1

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()
    print("categories list: ")
    print(count_line)
    print(total_categories_added)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


# next we insert checkins:
# e.g) checkins : {"time": {"Friday": {"20:00": 2, "19:00": 1, "22:00": 10, "21:00": 5, "23:00": 14, "0:00": 2, "18:00": 2}
# This is similar to the hours list, so we'll need a helper parse function


# CREATE TABLE Checkins (
# 	checkin_day		VARCHAR(10),
# 	counts		INTEGER,
# 	before_noon		TIME NOT NULL,
# 	after_noon		TIME NOT NULL,
# 	--times		TIME NOT NULL,  --We can't just use each individual otherwise insert will take hours, thus we simplify it into mornings and evenings
# 	business_id		CHAR(22),
# 	PRIMARY KEY(checkin_day, times, business_id),
# 	FOREIGN KEY(business_id) REFERENCES Business(business_id)
# );

def checkIfBeforeNoon(time): #If before 12:01, then True, else False
    temp = time.split(':', 1) #(separator, max)
    if(int(temp[0]) < 12):
        return True
    elif (int(temp[0]) == 12 and int(temp[1]) < 1): # 12:01 is the cutoff point
        return True
    else:
        return False

def parseCheckins(time, bus_id): #Creates the list of SQL commands (hours, businessID)
    returnListOfSQL = [] #initialize list of SQL commands to return
    counts = 0 #total checkins for a day
    morning = 0 #12am to 12pm
    evening = 0 #12pm to 12am
    for k,v in time.items(): # k (key) = Monday - Friday.   v(value) = {checkin times: # of checkins}
        #print('K: ' + k + ' V: ' + v.keys())
        for x,y in v.items(): # v(value) = {checkin times: # of checkins}
            if(checkIfBeforeNoon(x)):
                morning += y
            else:
                evening += y
            counts += y
            #print('x: ' + x + ' y: ' + str(y))
            # x = checkin time
            # y = # of checkins

        tempSQLCommand = "INSERT INTO Checkins (checkin_day, counts, before_noon, after_noon, business_id) " + \
                        "VALUES ('" + cleanStr4SQL(k) + "'," + str(counts) + "," + str(morning) + "," + str(evening) + ",'" + bus_id + "');"
        # reset counters
        counts = 0 
        morning = 0 
        evening = 0 
        returnListOfSQL.append(tempSQLCommand) 
    return returnListOfSQL




def insert2CheckinsTable():
    #reading the JSON file
    with open('./yelp_checkin.JSON','r') as f:    
        #outfile =  open('./yelp_checkins.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0
        total_checkins_added = 0
        #test = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # We'll need to parse checkins object for each line 
            checkinsSQL = parseCheckins(data['time'], cleanStr4SQL(data['business_id'])) #grab the time object and business_id at current line
            # if(test == 1):
            #     return
            # test += 1
            # Generate the INSERT statement for the current business hours
            # include values for all checkinsTable attributes
            for item in checkinsSQL:
                sql_str = item
                try:
                    cur.execute(sql_str)
                except psycopg2.Error as e:
                    print("Insert to checkinsTABLE failed!")
                    print("Error Message: ", e)
                    return #if we get a single failure or error, end the loop
                conn.commit()
                # optionally you might write the INSERT statement to a file.
                #outfile.write(sql_str)
                total_checkins_added += 1

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()
    print("checkins list: ")
    print(count_line)
    print(total_checkins_added)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2UserTable():
    #reading the JSON file
    with open('./yelp_user.JSON','r') as f:    
        #outfile =  open('./yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # include values for all userTable attributes
            sql_str = "INSERT INTO Users (user_id, average_stars, cool, funny, useful, user_name, fans, review_count, yelping_since) " + \
                      "VALUES ('" + cleanStr4SQL(data['user_id']) + "'," + str(data['average_stars']) + "," + str(data['cool']) + \
                      "," + str(data['funny']) + "," + str(data['useful']) + ",'" + cleanStr4SQL(data['name']) + "'," + str(data['fans']) + "," + \
                      str(data['review_count']) + ",'" + cleanStr4SQL(data['yelping_since']) + "');" 
                       
            try:
                cur.execute(sql_str)
            except psycopg2.Error as e:
                print("Insert to userTABLE failed!")
                print("Error Message: ", e) 
                break  #if we get a single failure or error, end the loop
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            #outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()
    print("user list: ")
    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2ReviewTable():
    #reading the JSON file
    with open('./yelp_review.JSON','r') as f:    
        #outfile =  open('./yelp_review.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent review
            # include values for all reviewTable attributes
            sql_str = "INSERT INTO Review (review_id, user_id, business_id, review_stars, dates, text, useful_vote, funny_vote, cool_vote) " + \
                      "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data['business_id']) + \
                      "'," + str(data['stars']) + ",'" + cleanStr4SQL(data['date']) + "','" + cleanStr4SQL(data['text']) + "'," + str(data['useful']) + "," + \
                      str(data['funny']) + "," + str(data['cool']) + ");" 
                       
            try:
                cur.execute(sql_str)
            except psycopg2.Error as e:
                print("Insert to reviewTABLE failed!")
                print("Error Message: ", e) 
                break  #if we get a single failure or error, end the loop
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            #outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()
    print("review list: ")
    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2FriendTable():
    #reading the JSON file
    with open('./yelp_user.JSON','r') as f:    
        #outfile =  open('./yelp_friend.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0
        total_friends_added = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent friend
            # include values for all friendTable attributes

            #friends is a object inside users
            # We must use similar method as hour and checkin

            for item in data['friends']:
                sql_str = "INSERT INTO Friends (friend_id, user_id) " + \
                        "VALUES ('" + str(item) + "','" + cleanStr4SQL(data['user_id']) + "');" 
                        
                try:
                    cur.execute(sql_str)
                except psycopg2.Error as e:
                    print("Insert to friendTABLE failed!")
                    print("Error Message: ", e) 
                    break  #if we get a single failure or error, end the loop
                conn.commit()
                # optionally you might write the INSERT statement to a file.
                #outfile.write(sql_str)
                total_friends_added += 1

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()
    print("friend list: ")
    print(count_line)
    print(total_friends_added)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


insert2BusinessTable()
insert2HoursTable()
insert2CategoriesTable()
insert2CheckinsTable()
insert2UserTable()
insert2ReviewTable()
insert2FriendTable()


-- CREATE OR REPLACE FUNCTION defineDept() RETURNS trigger AS '
-- BEGIN 
   -- INSERT INTO Dept
   -- (SELECT New.dno, NULL as dname, NULL as mgr
    -- WHERE (NOT EXISTS ( SELECT *
                        -- FROM Dept
            	          -- WHERE Dept.dno = NEW.dno)) );
   -- RETURN NEW;
-- END
-- ' LANGUAGE plpgsql; 


CREATE OR REPLACE FUNCTION incrementReviewCount() RETURNS trigger AS '
BEGIN 
   UPDATE Business
   SET review_count = review_count + 1
   WHERE Business.business_id = NEW.business_id;
   RETURN NEW;
END
' LANGUAGE plpgsql; 


CREATE TRIGGER updateReviewCount
	AFTER INSERT ON Review
	FOR EACH ROW
	EXECUTE PROCEDURE incrementReviewCount();
	
-- CREATE OR REPLACE FUNCTION calculateReviewRating() RETURN trigger AS '
-- BEGIN
	-- UPDATE Business
	-- SET review_rating = (TotalReviews.sum / TotalReviews2.count)
	-- FROM (select business_id, sum(review_stars) FROM Review GROUP BY business_id) as TotalReviews,
	-- (select business_id, count(review_stars) FROM Review GROUP BY business_id) as TotalReviews2
	-- WHERE Business.business_id = TotalReviews.business_id AND TotalReviews.business_id = TotalReviews2.business_id;
	-- RETURN NEW;
-- END
-- ' LANGUAGE plpgsql;
	
-- CREATE TRIGGER updateReviewRating
	-- AFTER INSERT ON Review
	-- FOR EACH ROW
	-- EXECUTE PROCEDURE calculateReviewRating();
	
CREATE TRIGGER updateReviewRating
	AFTER INSERT ON Review
	SET review_rating = SUM(SELECT review_stars FROM Review WHERE Review.business_id = business_id) / COUNT(SELECT review_id FROM Review WHERE Review.business_id = business_id);
	
	
CREATE OR REPLACE FUNCTION incrementCheckins() RETURNS trigger AS '
BEGIN
	UPDATE Business
	SET num_checkins = num_checkins + 1
	WHERE Business.business_id = NEW.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql;


CREATE TRIGGER updateCheckins
	AFTER INSERT ON Checkins
	FOR EACH ROW
	EXECUTE PROCEDURE incrementCheckins();
	
--TEST

-- INSERT INTO Review(review_id,user_id,business_id,review_stars,dates,text)
-- VALUES ('ClgrKJ6dqiM7vSKJBJ2w6q','om5ZiponkpRqUNa3pVPiRg','--ab39IjZR_xUf81WyTyHg',5,'2019-03-22','test');

-- INSERT INTO Checkins(checkin_day,counts,before_noon,after_noon,business_id)
-- VALUES ('Friday',12,7,5,'--ab39IjZR_xUf81WyTyHg');
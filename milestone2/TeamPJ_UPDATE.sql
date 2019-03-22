/* NUMCHECKINS */

UPDATE Business
SET num_checkins = total.SUM
FROM(SELECT SUM(counts), business_id FROM Checkins GROUP BY business_id) as total
WHERE Business.business_id = total.business_id;



/* REVIEWCOUNT */

UPDATE Business
SET business_review_count = total.COUNT
FROM (SELECT count(review_stars), business_id FROM Review GROUP BY business_id) as total
WHERE Business.business_id = total.business_id;


/* REVIEWRATING */

UPDATE Business
SET review_rating = (TotalReviews.sum / TotalReviews2.count)
FROM (select business_id, sum(review_stars) FROM Review GROUP BY business_id) as TotalReviews,
(select business_id, count(review_stars) FROM Review GROUP BY business_id) as TotalReviews2
WHERE Business.business_id = TotalReviews.business_id AND TotalReviews.business_id = TotalReviews2.business_id;
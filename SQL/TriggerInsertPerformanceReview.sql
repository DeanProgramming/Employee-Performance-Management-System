CREATE TRIGGER Trigger_AfterInsert_PerformanceReview
ON Employee.PerformanceReview
AFTER INSERT
AS
BEGIN
    INSERT INTO Employee.PerformanceReviewAudit (review_id, employee_id, review_date, score, comments, operation_type)
    SELECT review_id, employee_id, review_date, score, comments, 'INSERT'
    FROM inserted;
END;

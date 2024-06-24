CREATE TRIGGER Trigger_AfterUpdate_PerformanceReview
ON Employee.PerformanceReview
AFTER UPDATE
AS
BEGIN
    INSERT INTO Employee.PerformanceReviewAudit (review_id, employee_id, review_date, score, comments, operation_type)
    SELECT i.review_id, i.employee_id, i.review_date, i.score, i.comments, 'UPDATE'
    FROM inserted i
    INNER JOIN deleted d ON i.review_id = d.review_id;
END;

CREATE PROCEDURE UpdatePerformanceReview
	@ReviewID INT, 
    @ReviewDate DATE,
    @Score INT,
    @Comments VARCHAR(255)
AS
BEGIN 
    SET NOCOUNT ON;
	BEGIN TRY
        -- Update performance review in Employee.PerformanceReview table
        UPDATE Employee.PerformanceReview
        SET score = @Score,
			comments = @Comments,
			review_date = @ReviewDate
		WHERE review_id = @ReviewID; 
        
        -- Check if any rows were affected
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR ('Performance review not found or details unchanged.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        -- Rollback the transaction if one is active
        IF @@TRANCOUNT > 0
            ROLLBACK;

        -- Raise the error
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;
go
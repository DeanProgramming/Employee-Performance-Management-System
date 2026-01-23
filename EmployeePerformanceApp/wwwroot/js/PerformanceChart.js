(function () {

    if (!window.teamPerformanceData || !document.getElementById('performanceChart')) {
        return;
    }

    const teamPerformanceData = window.teamPerformanceData;

    // Extract latest review per employee safely
    const latestReviews = teamPerformanceData
        .filter(e => e.Reviews && e.Reviews.length > 0)
        .map(employee => {
            const latestReview = employee.Reviews[employee.Reviews.length - 1];
            return {
                employeeName: employee.EmployeeName,
                score: latestReview.Score
            };
        });

    if (latestReviews.length === 0) return;

    const average =
        latestReviews.reduce((sum, r) => sum + r.score, 0) / latestReviews.length;

    const ctx = document.getElementById('performanceChart').getContext('2d');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: latestReviews.map(r => r.employeeName),
            datasets: [{
                label: 'Latest Performance Review Score',
                data: latestReviews.map(r => r.score),
                backgroundColor: latestReviews.map((r, i) =>
                    r.score < 70
                        ? 'rgba(220, 53, 69, 0.8)' // nicer Bootstrap red
                        : `rgba(13, 110, 253, ${0.3 + (i % 5) * 0.1})`
                ),
                borderColor: latestReviews.map(r =>
                    r.score < 70 ? 'rgba(220, 53, 69, 1)' : 'rgba(13, 110, 253, 1)'
                ),
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: { beginAtZero: true }
            },
            plugins: {
                annotation: {
                    annotations: {
                        cutoff: {
                            type: 'line',
                            yMin: 70,
                            yMax: 70,
                            borderColor: 'black',
                            borderWidth: 2,
                            label: {
                                enabled: true,
                                content: 'Cutoff: 70',
                                backgroundColor: 'rgba(0,0,0,0.8)',
                                color: 'white'
                            }
                        },
                        avg: {
                            type: 'line',
                            yMin: average,
                            yMax: average,
                            borderColor: 'black',
                            borderWidth: 2,
                            label: {
                                enabled: true,
                                content: 'Average: ' + Math.round(average),
                                backgroundColor: 'rgba(0,0,0,0.8)',
                                color: 'white'
                            }
                        }
                    }
                }
            }
        }
    });

})();

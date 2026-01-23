(function () {

    const payload = window.performanceChartData;
    if (!payload) {
        console.warn("performanceChartData not found on window.");
        return;
    } 

    const EmployeeData = Array.isArray(payload.employeeData) ? [...payload.employeeData] : [];
    const EmployeeName = payload.employeeName ?? "Employee";

    if (EmployeeData.length === 0) {
        console.warn("No performance review data to chart.");
        return;
    }

    // average score
    let average = 0;
    for (let i = 0; i < EmployeeData.length; i++) {
        average += EmployeeData[i].score;
    }
    average = average / EmployeeData.length;

    EmployeeData.reverse();
    const canvas = document.getElementById("performanceChart");
    if (!canvas) {
        console.warn("#performanceChart canvas not found.");
        return;
    }

    const ctx = canvas.getContext("2d"); 

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: EmployeeData.map(review => {
                const date = new Date(review.review_date);
                return date.toLocaleDateString('en-GB', {
                    day: '2-digit',
                    month: 'short',
                    year: 'numeric'
                });
            }),
            datasets: [{
                label: EmployeeName,
                data: EmployeeData.map(review => {
                    return {
                        x: new Date(review.review_date),
                        y: review.score
                    };
                }),
                backgroundColor: 'rgba(75, 192, 192, 0.9)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: false
                }
            },
            plugins: {
                annotation: {
                    annotations: {
                        line1: {
                            type: 'line',
                            yMin: average,
                            yMax: average,
                            borderColor: 'black',
                            borderWidth: 2,
                            label: {
                                enabled: true,
                                content: 'Average at ' + Math.round(average),
                                position: 'end',
                                backgroundColor: 'rgba(0,0,0,0.8)',
                                color: 'white',
                                font: {
                                    style: 'bold'
                                }
                            }
                        }
                    }
                }
            }
        }
    });
})();

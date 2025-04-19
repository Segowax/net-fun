const apiUrl = "https://my-wonderful-api-acgvdvaaa5d4b9ez.northeurope-01.azurewebsites.net/api/sensor/gettemperature";

const ctx = document.getElementById('temperatureChart').getContext('2d');
const chart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [], // Time labels
        datasets: [{
            label: 'Temperature (°C)',
            data: [], // Temperature values
            borderColor: 'rgba(75, 192, 192, 1)',
            backgroundColor: 'rgba(75, 192, 192, 0.2)',
            borderWidth: 1,
            tension: 0.4
        }]
    },
    options: {
        responsive: true,
        plugins: {
            legend: {
                display: true,
                position: 'top',
                labels: {
                    font: {
                        size: 12
                    }
                }
            }
        },
        scales: {
            x: {
                title: {
                    display: true,
                    text: 'Time'
                }
            },
            y: {
                title: {
                    display: true,
                    text: 'Temperature (°C)'
                },
                beginAtZero: false
            }
        }
    }
});

// Add date pickers for filtering data
const startDateInput = document.getElementById('startDate');
const endDateInput = document.getElementById('endDate');

// Set default values for date pickers
const now = new Date();
const oneWeekAgo = new Date();
oneWeekAgo.setDate(now.getDate() - 7);

startDateInput.value = oneWeekAgo.toISOString().slice(0, 10);
endDateInput.value = now.toISOString().slice(0, 10);

// Add a legend to display the sensor name
const legendContainer = document.createElement('div');
legendContainer.id = 'legend';
legendContainer.style.marginTop = '10px';
legendContainer.style.fontWeight = 'bold';
document.body.appendChild(legendContainer);

async function fetchTemperatureData() {
    try {
        const response = await fetch(apiUrl);
        const data = await response.json();

        // Filter data based on selected date range
        const startDate = new Date(startDateInput.value);
        const endDate = new Date(endDateInput.value);
        endDate.setHours(23, 59, 59, 999); // Include the entire end day

        const filteredData = data.filter(entry => {
            const entryDate = new Date(entry.enqueuedTime);
            return entryDate >= startDate && entryDate <= endDate;
        });

        // Extract the latest data points
        const labels = filteredData.map(entry => {
            const entryDate = new Date(entry.enqueuedTime);
            return `${entryDate.toLocaleDateString()} ${entryDate.toLocaleTimeString()}`;
        });
        const values = filteredData.map(entry => entry.value);

        // Update the chart
        chart.data.labels = labels;
        chart.data.datasets[0].data = values;

        // Update the legend label with sensorId
        if (data.length > 0 && data[0].sensorId) {
            chart.data.datasets[0].label = `Temperature (°C) - Sensor: ${data[0].sensorId}`;
        } else {
            chart.data.datasets[0].label = 'Temperature (°C) - Sensor: Unknown';
        }

        chart.update();
    } catch (error) {
        console.error("Error fetching temperature data:", error);
    }
}

fetchTemperatureData();
setInterval(fetchTemperatureData, 60000);

window.addEventListener('resize', () => {
    chart.resize();
});

startDateInput.addEventListener('change', fetchTemperatureData);
endDateInput.addEventListener('change', fetchTemperatureData);
const apiUrl = "https://my-wonderful-api-acgvdvaaa5d4b9ez.northeurope-01.azurewebsites.net/api/sensor/gettemperature";
const lockStateUrl = "https://my-wonderful-api-acgvdvaaa5d4b9ez.northeurope-01.azurewebsites.net/api/sensor/getcurrentlockstate";

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

        const filteredByDate = data.filter(entry => {
            const entryDate = new Date(entry.enqueuedTime);
            return entryDate >= startDate && entryDate <= endDate;
        });

        // Group data by sensorId
        const groupBySensorId = filteredByDate.reduce((acc, curr) => {
            const key = curr.sensorId;
            if (!acc[key]) {
                acc[key] = [];
            }
            acc[key].push(curr);
            return acc;
        }, {});

        // Create a unified timeline (all unique timestamps)
        const unifiedTimeline = Array.from(
            new Set(filteredByDate.map(entry => entry.enqueuedTime))
        ).sort((a, b) => new Date(a) - new Date(b));

        // Generate unique colors for each sensor
        const colors = [
            'rgba(75, 192, 192, 1)',
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(153, 102, 255, 1)'
        ];

        // Update chart datasets
        chart.data.datasets = Object.keys(groupBySensorId).map((sensorId, index) => {
            const sensorData = groupBySensorId[sensorId];
            const sensorDataMap = sensorData.reduce((acc, entry) => {
                acc[entry.enqueuedTime] = entry.value;
                return acc;
            }, {});

            // Align data to the unified timeline
            const alignedData = unifiedTimeline.map(timestamp => sensorDataMap[timestamp] || null);

            return {
                label: `Temperature (°C) - Sensor: ${sensorId}`,
                data: alignedData,
                borderColor: colors[index % colors.length],
                backgroundColor: colors[index % colors.length].replace('1)', '0.2)'),
                borderWidth: 1,
                tension: 0.4,
                spanGaps: true // Enable interpolation of missing data points
            };
        });

        // Update chart labels (unified timeline)
        chart.data.labels = unifiedTimeline.map(timestamp => {
            const entryDate = new Date(timestamp);
            const localDate = new Date(entryDate.getTime() - entryDate.getTimezoneOffset() * 60000); // Convert UTC to local time

            return `${localDate.toLocaleDateString()} ${localDate.toLocaleTimeString()}`;
        });

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

const lockStateIcon = document.getElementById('lockStateIcon');
const lockStateText = document.getElementById('lockStateText');

async function fetchLockState() {
    try {
        const response = await fetch(lockStateUrl);
        const data = await response.json();

        if (data.value === "Open") {
            lockStateIcon.classList.remove('closed');
            lockStateIcon.classList.add('open');
            lockStateText.textContent = "Lock State: Open";
        } else if (data.value === "Closed") {
            lockStateIcon.classList.remove('open');
            lockStateIcon.classList.add('closed');
            lockStateText.textContent = "Lock State: Closed";
        }
    } catch (error) {
        console.error("Error fetching lock state:", error);
    }
}

fetchLockState();
setInterval(fetchLockState, 60000); // Refresh lock state every 60 seconds
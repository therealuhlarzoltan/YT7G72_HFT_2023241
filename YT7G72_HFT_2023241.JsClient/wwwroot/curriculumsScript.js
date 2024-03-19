let curriculums = [];

initialize();


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:4180/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("CurriculumCreated", (user, message) => {
        getCurriculums();
    });

    connection.on("CurriculumUpdated", (user, message) => {
        getCurriculums();
    });

    connection.on("CurriculumDeleted", (user, message) => {
        getCurriculums();
    });

    connection.onclose(async () => {
        await start();
    });
    start();


}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

async function getCurriculums() {
    await fetch('http://localhost:4180/Curriculums')
        .then(response => response.json())
        .then(data => {
            curriculums = data;
            displayCurriculums();
        });
}


function displayCurriculums() {
    const curriculumtRows = curriculums.map(c => `
        <tr>
            <td>${c.curriculumId}</td>
            <td>${c.curriculumName}</td>
            <td>${c.curriculumCode}</td>
             <td><button type="button" class="btn btn-info" onclick="displayUpdateCurriculum(${c.curriculumId})">Update</button><button type="button" class="btn btn-danger" onclick="removeCurriculum(${c.curriculumId})">Delete</button></td>
        </tr>`
    );
    document.getElementById('curriculums').innerHTML = curriculumtRows.join('');
}


async function removeStudent(curriculumId) {
    const deleteUrl = 'http://localhost:4180/Curriculums/' + curriculumId;
    const options = {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        },
    };

    try {
        const response = await fetch(deleteUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Curriculum deleted!");
            getCurriculums();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}



async function initialize() {
    getCurriculums();
    setupSignalR();
}

function displayUpdateCurriculum(curriculumId) {
    let curriculum = curriculums.find(c => c.curriculumId == curriculumId);
    document.getElementById("curriculumUpdateId").value = curriculumId;
    document.getElementById("curriculumUpdateCurriculumName").value = curriculum?.curriculumName;
    document.getElementById("curriculumUpdateCurriculumCode").value = curriculum?.curriculumCode;
    document.getElementById("curriculumUpdateButton").addEventListener("click", () => updateCurriculum(curriculumId));
    document.getElementById("curriculumCreateDiv").style.display = 'none';
    document.getElementById("curriculumUpdateDiv").style.display = 'block';

}

function cancelUpdate() {
    document.getElementById("curriculumUpdateDiv").style.display = 'none';
    document.getElementById("curriculumCreateDiv").style.display = 'block';
}

function displayErrorMessage(message) {
    document.getElementById("curriculumAlertDiv").innerHTML =
        `  
        <div class="alert alert-danger alert-dismissible fade show" role="alert">
            <div>
                <i class="bi bi-exclamation-triangle-fill"></i>
                <strong>${message}</strong>
            </div>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`
}

function displaySuccessMessage(message) {
    document.getElementById("curriculumAlertDiv").innerHTML =
        `  
        <div class="alert alert-success alert-dismissible fade show" role="alert">
            <div>
                <i class="bi bi-check-circle mx-2"></i>
                <strong>${message}</strong>
            </div>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`
}


async function createCurriculum() {
    const createUrl = 'http://localhost:4180/Curriculums';
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            CurriculumName: document.getElementById("curriculumCreateCurriculumName").value,
            CurriculumCode: document.getElementById("curriculumCreateCurriculumCode").value
        })
    };

    try {
        const response = await fetch(createUrl, options);
        if (!response.ok) {
            const data = await response.json();
            console.log("Response data: ", data);
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Curriculum created!");
            resetCreationForm();
            getCurriculums();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

async function updateCurriculum(curriculumId) {
    const updateUrl = 'http://localhost:4180/Curriculums'
    const options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            CurriculumId: Number(curriculumId),
            CurriculumName: document.getElementById("curriculumUpdateCurriculumName").value,
            CurriculumCode: document.getElementById("curriculumUpdateCurriculumCode").value
        })
    };


    try {
        const response = await fetch(updateUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Curriculum updated!");
            document.getElementById("curriculumCreateDiv").style.display = 'block';
            document.getElementById("curriculumUpdateDiv").style.display = 'none';
            getCurriculums();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}


function resetCreationForm() {
    document.getElementById("curriculumCreateCurriculumName").value = '';
    document.getElementById("curriculumCreateCurriculumCode").value = '';
}

async function removeCurriculum(curriculumId) {
    const deleteUrl = 'http://localhost:4180/Curriculums/' + curriculumId;
    const options = {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        },
    };

    try {
        const response = await fetch(deleteUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Curriculum deleted!");
            getCurriculums();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}


async function resetSemester() {
    const resetUrl = 'http://localhost:4180/Education/Semester/Reset'
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    };


    try {
        const response = await fetch(resetUrl, options);
        if (!response.ok) {
            displayErrorMessage("Failed to reset semester!");
        } else {
            displaySuccessMessage("Course and subject registrations cleared!");
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

async function getAllSemesterStatistics() {
    const statisticstUrl = 'http://localhost:4180/Grades/Semester/Statistics'
    const options = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    };


    try {
        const response = await fetch(statisticstUrl, options);
        if (!response.ok) {
            displayErrorMessage("Failed to get statistics!");
        } else {
            data = await response.json();
            displaySemesterStatistics(data, true);
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}

async function getSemesterStatistics() {

    semester = document.getElementById("semester").value;
    if (semester == '') {
        displayErrorMessage("Semester required!");
        return;
    }


    semester = semester.trim().split('/').join('-');


    const statisticstUrl = 'http://localhost:4180/Grades/Semester/Statistics/' + semester
    const options = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    };


    try {
        const response = await fetch(statisticstUrl, options);
        if (!response.ok) {
            displayErrorMessage("Failed to get statistics!");
        } else {
            data = await response.json();
            displaySemesterStatistics(data);
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}

function displaySemesterStatistics(data, isMultiple = false) {
    if (!isMultiple) {
        var element = document.createElement("div");
        element.innerHTML = `<div class="col-md-6">
            <div class="statistics-card">
                <h4>Semester: ${data.semester}</h4>
                <p>Weighted Average: ${data.weightedAvg}</p>
                <p>Number of Failures: ${data.numberOfFailures}</p>
                <p>Number of Passes: ${data.numberOfPasses}</p>
            </div>
        </div>`
        document.getElementById("semesterStatisticsDiv").innerHTML = '';
        document.getElementById("semesterStatisticsDiv").appendChild(element);
    } else {
        document.getElementById("allSemesterStatisticsDiv").innerHTML = '';
        data.forEach((ss) => {
            var element = document.createElement("div");
            element.innerHTML = `<div class="col-md-6">
            <div class="statistics-card">
                <h4>Semester: ${ss.semester}</h4>
                <p>Weighted Average: ${ss.weightedAvg}</p>
                <p>Number of Failures: ${ss.numberOfFailures}</p>
                <p>Number of Passes: ${ss.numberOfPasses}</p>
            </div>
        </div>`
            document.getElementById("allSemesterStatisticsDiv").appendChild(element);
        });
    }
}



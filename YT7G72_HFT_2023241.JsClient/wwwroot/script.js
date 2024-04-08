let students = [];
let curriculums = [];
let isScheduleShowing = false;
let scheduleStudentId = null;
financialStatuses = [
    { ordinal: 0, value: 'Without Scholarship' },
    { ordinal: 1, value: 'Full State Scholarship' }
]

initialize();


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:4180/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("StudentCreated", (user, message) => {
        getStudents();
    });

    connection.on("StudentUpdated", (user, message) => {
        getStudents();
    });

    connection.on("StudentDeleted", (user, message) => {
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("CurriculumUpdated", async (user, message) => {
        await getCurriculums();
        getStudents();
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("CurriculumCreated", async (user, message) => {
        await getCurriculums();
        getStudents();
    });

    connection.on("CurriculumDeleted", async (user, message) => {
        await getCurriculums();
        getStudents();
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("CourseUpdated", async (user, message) => {
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("TeacherDeleted", async (user, message) => {
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("TeacherUpdated", async (user, message) => {
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("CourseDeleted", async (user, message) => {
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("SubjectUpdated", async (user, message) => {
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
    });

    connection.on("SubjectDeleted", async (user, message) => {
        if (isScheduleShowing == true && scheduleStudentId != null) {
            getSchedule(scheduleStudentId);
        }
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

async function getStudents() {
    await fetch('http://localhost:4180/People/Students')
        .then(response => response.json())
        .then(data => {
            students = data;
            displayStudents();
        });
}

async function getCurriculums() {
    await fetch('http://localhost:4180/Curriculums')
        .then(response => response.json())
        .then(data => {
            curriculums = data;
            generateCurriculumOptions("studentCreateCurriculum");
            generateCurriculumOptions("studentUpdateCurriculum");
        });
}

function generateCurriculumOptions(selectId) {
    let option = document.getElementById(selectId)
    curriculums.forEach((c) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", c.curriculumId);
        newChild.innerText = c.curriculumName;
        option.appendChild(newChild);
    });
}

function displayStudents() {
    const studentRows = students.map(s => `
        <tr>
            <td>${s.studentId}</td>
            <td>${s.studentCode}</td>
            <td>${s.firstName}</td>
            <td>${s.lastName}</td>
            <td>${financialStatusToString(s.financialStatus)}</td>
            <td>${curriculumToString(s.curriculumId)}</td>
             <td><button type="button" class="btn btn-info" onclick="displayUpdateStudent(${s.studentId})">Update</button>
             <button type="button" class="btn btn-danger" onclick="removeStudent(${s.studentId})">Delete</button>
             <button type="button" class="btn btn-success" onclick="getSchedule(${s.studentId})">Display Schedule</button></td>
        </tr>`);
    document.getElementById('students').innerHTML = studentRows.join('');
}


async function getSchedule(studentId) {
    await fetch('http://localhost:4180/People/Students/Schedule/' + studentId)
        .then(response => response.text())
        .then(data => displaySchedule(data));
    scheduleStudentId = studentId;
}

function convertOrdinalToDay(ordinal) {
    switch (ordinal) {
        case 0:
            return 'Monday:';
        case 1:
            return 'Tuesday:';
        case 2:
            return 'Wednesday:';
        case 3:
            return 'Thursday:';
        case 4:
            return 'Friday:';
        case 5:
            return 'Saturday:';
        case 6:
            return 'Sunday:';
        default:
            return 'Monday:';
    }
}

function displaySchedule(schedule) {
    const tbody = document.getElementById("schedule");
    tbody.innerHTML = '';
    document.getElementById("scheduleDisplayDiv").style.display = 'block';
    const classes = {};
    const lines = schedule.split('\n');
    let currentDay = '';

    for (const line of lines) {
        if (line.trim() !== '') {
            if (line.endsWith(':')) {
                currentDay = line.trim();
                classes[currentDay] = [];
            } else {
                classes[currentDay].push(line.trim());
            }
        }
    }

    numberOfIterations = 0;
    maxLengthOfDays = 0;
    for (let key of Object.keys(classes)) {
        let classArray = classes[key];
        if (classArray.length > maxLengthOfDays)
            maxLengthOfDays = classArray.length;
    }

    let classRows = [];
    for (let i = 0; i < maxLengthOfDays / 6; ++i) {
        let row = '<tr>';
        for (let j = 0; j < 7; ++j) {
            let day = classes[convertOrdinalToDay(j)];
            let td = '';
            if (day.length == 0 || classes.length / 6 < numberOfIterations)
                td = '<td></td>';
            else {
                td += '<td >';
                for (let k = numberOfIterations * 6; (k < classes[convertOrdinalToDay(j)].length) && (k < numberOfIterations * 6 + 6); ++k) {
                    td += day[k];
                    td += ' ';
                }
                td += '</td>';
            }
            row += td;
        }
        row += '</tr>';
        classRows[classRows.length] = row;
        numberOfIterations += 1;
    }
    classRows.forEach(r => tbody.innerHTML += r);
    isScheduleShowing = true;
    window.scrollTo(0, document.body.scrollHeight);

}

async function removeStudent(studentId) {
    const deleteUrl = 'http://localhost:4180/People/Students/' + studentId;
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
            displaySuccessMessage("Student deleted!");
            getStudents();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

function curriculumToString(curriculumId) {
    const foundCurriculum = curriculums.find(c => c.curriculumId == curriculumId);
    if (foundCurriculum != null)
        return foundCurriculum.curriculumName;
    return "Unknown";
}

function financialStatusToString(ordinal) {
    const foundFinancialStatus = financialStatuses.find(fs => fs.ordinal == ordinal);
    if (foundFinancialStatus != null)
        return foundFinancialStatus.value;

    return "Unknown";
} 

async function initialize() {
    await getCurriculums();
    setupSignalR();
    getStudents();
}

function displayUpdateStudent(studentId) {
    let student = students.find(s => s.studentId == studentId);
    document.getElementById("studentUpdateId").value = studentId;
    document.getElementById("studentUpdateFirstName").value = student?.firstName;
    document.getElementById("studentUpdateLastName").value = student?.lastName;
    document.getElementById("studentUpdateFinancialStatus").value = student?.financialStatus;
    document.getElementById("studentUpdateCurriculum").value = student?.curriculumId;
    document.getElementById("studentUpdateStudentCode").value = student?.studentCode;
    document.getElementById("studentUpdateButton").addEventListener("click", () => updateStudent(studentId));
    document.getElementById("studentCreateDiv").style.display = 'none';
    document.getElementById("studentUpdateDiv").style.display = 'block';

}

function cancelUpdate() {
    document.getElementById("studentUpdateDiv").style.display = 'none';
    document.getElementById("studentCreateDiv").style.display = 'block';
}

function displayErrorMessage(message) {
    document.getElementById("studentAlertDiv").innerHTML =
        `  
        <div class="alert alert-danger alert-dismissible fade show" role="alert">
            <div>
                <i class="bi bi-exclamation-triangle-fill"></i>
                <strong>${message}</strong>
            </div>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`
    window.scrollTo(0, 0);
}

function displaySuccessMessage(message) {
    document.getElementById("studentAlertDiv").innerHTML =
        `  
        <div class="alert alert-success alert-dismissible fade show" role="alert">
            <div>
                <i class="bi bi-check-circle mx-2"></i>
                <strong>${message}</strong>
            </div>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`
    window.scrollTo(0, 0);
}


async function createStudent() {
    const createUrl = 'http://localhost:4180/People/Students';
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            FirstName: document.getElementById("studentCreateFirstName").value,
            LastName: document.getElementById("studentCreateLastName").value,
            StudentCode: document.getElementById("studentCreateStudentCode").value,
            FinancialStatus: Number(document.getElementById("studentCreateFinancialStatus").value),
            CurriculumId: Number(document.getElementById("studentCreateCurriculum").value),
        })
    };

    try {
        const response = await fetch(createUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Student created!");
            resetCreationForm();
            getStudents();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

async function updateStudent(studentId) {
    const updateUrl = 'http://localhost:4180/People/Students/'
    const options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            StudentId: Number(studentId),
            FirstName: document.getElementById("studentUpdateFirstName").value,
            LastName: document.getElementById("studentUpdateLastName").value,
            StudentCode: document.getElementById("studentUpdateStudentCode").value,
            FinancialStatus: Number(document.getElementById("studentUpdateFinancialStatus").value),
            CurriculumId: Number(document.getElementById("studentUpdateCurriculum").value),
        })
    };


    try {
        const response = await fetch(updateUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Student updated!");
            document.getElementById("studentCreateDiv").style.display = 'block';
            document.getElementById("studentUpdateDiv").style.display = 'none';
            getStudents();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}


function resetCreationForm() {
    document.getElementById("studentCreateFirstName").value = '';
    document.getElementById("studentCreateLastName").value = '';
    document.getElementById("studentCreateStudentCode").value = '';
    document.getElementById("studentCreateFinancialStatus").value = 0;
    document.getElementById("studentCreateCurriculum").value = 0;
}

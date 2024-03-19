let teachers = [];

setupSignalR();
getTeachers();


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:4180/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("TeacherCreated", (user, message) => {
        getTeachers();
    });

    connection.on("TeachertUpdated", (user, message) => {
        getTeachers();
    });

    connection.on("TeacherDeleted", (user, message) => {
        getTeachers();
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

async function getTeachers() {
    await fetch('http://localhost:4180/People/Teachers')
        .then(response => response.json())
        .then(data => {
            teachers = data;
            displayTeachers();
        });
}

function displayTeachers() {
    const teacherRows = teachers.map(t => `
        <tr>
            <td>${t.teacherId}</td>
            <td>${t.firstName}</td>
            <td>${t.lastName}</td>
            <td>${convertAcademicRankToString(t.academicRank)}</td>
            <td><button type="button" class="btn btn-info" onclick="displayUpdateTeacher(${t.teacherId})">Update</button>
            <button type="button" class="btn btn-danger" onclick="removeTeacher(${t.teacherId})">Delete</button>
            <button type="button" class="btn btn-success" onclick="getSchedule(${t.teacherId})">Display Schedule</button></td></td>
        </tr>`
    );
    document.getElementById('teachers').innerHTML = teacherRows.join('');
}

function convertAcademicRankToString(academicRank) {
    switch (academicRank) {
        case 0:
            return "Teachers Assistant";
        case 1:
            return "Technical Assistant";
        case 2:
            return "Senior Lecturer";
        case 3:
            return "Associate Professor";
        case 4:
            return "Professor";
        default:
            return "Unknown";
    }
}


async function removeTeacher(teacherId) {
    const deleteUrl = 'http://localhost:4180/People/Teachers/' + teacherId;
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
            displaySuccessMessage("Teacher deleted!");
            getTeachers();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

function resetCreationForm() {
    document.getElementById("teacherCreateFirstName").value = '';
    document.getElementById("teacherCreateLastName").value = '';
    document.getElementById("teacherCreateAcademicRank").value = 0;
}

async function updateTeacher(teacherId) {
    const updateUrl = 'http://localhost:4180/People/Teachers'
    const options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            teacherId: Number(teacherId),
            firstName: document.getElementById("teacherUpdateFirstName").value,
            lastName: document.getElementById("teacherUpdateLastName").value,
            academicRank: Number(document.getElementById("teacherUpdateAcademicRank").value)
        })
    };


    try {
        const response = await fetch(updateUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Teacher updated!");
            document.getElementById("teacherCreateDiv").style.display = 'block';
            document.getElementById("teacherUpdateDiv").style.display = 'none';
            getTeachers();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

async function createTeacher() {
    const  createUrl = 'http://localhost:4180/People/Teachers';
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            FirstName: document.getElementById("teacherCreateFirstName").value,
            LastName: document.getElementById("teacherCreateLastName").value,
            AcademicRank: Number(document.getElementById("teacherCreateAcademicRank").value)
        })
    };

    try {
        const response = await fetch(createUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Teacher created!");
            resetCreationForm();
            getTeachers();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}


function displayUpdateTeacher(teacherId) {
    let teacher = teachers.find(t => t.teacherId == teacherId);
    document.getElementById("teacherUpdateId").value = teacherId;
    document.getElementById("teacherUpdateFirstName").value = teacher?.firstName;
    document.getElementById("teacherUpdateLastName").value = teacher?.lastName;
    document.getElementById("teacherUpdateAcademicRank").value = teacher?.academicRank;
    document.getElementById("teacherUpdateButton").addEventListener("click", () => updateTeacher(teacherId));
    document.getElementById("teacherCreateDiv").style.display = 'none';
    document.getElementById("teacherUpdateDiv").style.display = 'block';

}

function cancelUpdate() {
    document.getElementById("teacherUpdateDiv").style.display = 'none';
    document.getElementById("teacherCreateDiv").style.display = 'block';
}

function displayErrorMessage(message) {
    document.getElementById("teacherAlertDiv").innerHTML =
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
    document.getElementById("teacherAlertDiv").innerHTML = 
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


async function getSchedule(teacherId) {
    await fetch('http://localhost:4180/People/Teachers/Schedule/' + teacherId)
        .then(response => response.text())
        .then(data => displaySchedule(data));
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
    console.log(classes);

    let classRows = [];
    for (let i = 0; i < maxLengthOfDays / 5; ++i) {
        let row = '<tr>';
        for (let j = 0; j < 7; ++j) {
            let day = classes[convertOrdinalToDay(j)];
            let td = '';
            if (day.length == 0 || classes.length / 5 < numberOfIterations)
                td = '<td></td>';
            else {
                td += '<td >';
                for (let k = numberOfIterations * 5; (k < classes[convertOrdinalToDay(j)].length) && (k < numberOfIterations * 5 + 5); ++k) {
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

    window.scrollTo(0, document.body.scrollHeight);

}
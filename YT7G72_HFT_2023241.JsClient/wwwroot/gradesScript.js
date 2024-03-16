let teachers = [];
let subjects = [];
let grades = [];
let students = [];

initialize();

async function updateGrade(gradeId) {
    const updateUrl = 'http://localhost:4180/Grades'
    let options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            GradeId: Number(gradeId),
            Semester: document.getElementById("gradeUpdateSemester").value,
            StudentId: Number(document.getElementById("gradeUpdateStudent").value),
            TeacherId: Number(document.getElementById("gradeUpdateTeacher").value),
            SubjectId: Number(document.getElementById("gradeUpdateSubject").value),
            Mark: Number(document.getElementById("gradeUpdateMark").value)
        })
    };


    try {
        const response = await fetch(updateUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Grade updated!");
            document.getElementById("gradeCreateDiv").style.display = 'block';
            document.getElementById("gradeUpdateDiv").style.display = 'none';
            getGrades();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

async function createGrade() {
    const createUrl = 'http://localhost:4180/Grades';
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Semester: document.getElementById("gradeCreateSemester").value,
            StudentId: Number(document.getElementById("gradeCreateStudent").value),
            TeacherId: Number(document.getElementById("gradeCreateTeacher").value),
            SubjectId: Number(document.getElementById("gradeCreateSubject").value),
            Mark: Number(document.getElementById("gradeCreateMark").value)
        })
    };


    try {
        const response = await fetch(createUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Grade created!");
            resetCreationForm();
            getGrades();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}

async function removeGrade(gradeId) {
    const deleteUrl = 'http://localhost:4180/Grades/' + gradeId;
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
            displaySuccessMessage("Grade deleted!");
            getGrades();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

function convertTeacherToString(teacherId) {
    const foundTeacher = teachers.find(t => t.teacherId == teacherId);
    if (foundTeacher != null)
        return foundTeacher.firstName + ' ' + foundTeacher.lastName;
    return "Unknown";
}

function displayGrades() {
    const gradeRows = grades.map(g => `
        <tr>
            <td>${g.gradeId}</td>
            <td>${g.semester}</td> 
            <td>${g.student.firstName} ${g.student.lastName}</td>
            <td>${g.subject.subjectName}</td>
            <td>${convertTeacherToString(g.teacherId)}</td>
            <td>${g.mark}</td>
            <td><button type="button" class="btn btn-info" onclick="displayUpdateGrade(${g.gradeId})">Update</button><button type="button" class="btn btn-danger" onclick="removeGrade(${g.gradeId})">Delete</button></td>
        </tr>`
    );
    document.getElementById('grades').innerHTML = gradeRows.join('');
}

function generateSubjectOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    subjects.forEach((s) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", s.subjectId);
        newChild.innerText = s.subjectName;
        option.appendChild(newChild);
    });

}

function generateTeacherOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    teachers.forEach((t) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", t.teacherId);
        newChild.innerText = t.firstName + ' ' + t.lastName;
        option.appendChild(newChild);
    });
}

function generateStudentOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    students.forEach((s) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", s.studentId);
        newChild.innerText = s.firstName + ' ' + s.lastName;
        option.appendChild(newChild);
    });
}


function resetCreationForm() {
    document.getElementById("gradeCreateSemester").value = '';
    document.getElementById("gradeCreateStudent").value = document.getElementById("gradeCreateStudent").children[0].value;
    document.getElementById("gradeCreateSubject").value = document.getElementById("gradeCreateSubject").children[0].value;
    document.getElementById("gradeCreateTeacher").value = document.getElementById("gradeCreateTeacher").children[0].value;;
    document.getElementById("gradeCreateMark").value = 0;
    document.getElementById("gradeCreateSubject").value = document.getElementById("gradeCreateSubject").children[0].value;


}

function displayUpdateGrade(gradeId) {
    const grade = grades.find(g => g.gradeId == gradeId);
    document.getElementById("gradeUpdateId").value = gradeId;
    document.getElementById("gradeUpdateSemester").value = grade?.semester;
    document.getElementById("gradeUpdateStudent").value = grade?.studentId;
    document.getElementById("gradeUpdateSubject").value = grade?.subjectId;
    document.getElementById("gradeUpdateTeacher").value = grade?.teacherId;
    document.getElementById("gradeUpdateMark").value = grade?.mark;
    document.getElementById("gradeCreateDiv").style.display = 'none';
    document.getElementById("gradeUpdateDiv").style.display = 'block';
    document.getElementById("gradeUpdateButton").addEventListener("click", () => updateGrade(gradeId));

}

async function getGrades() {
    await fetch('http://localhost:4180/Grades')
        .then(response => response.json())
        .then(data => {
            console.log(data);
            grades = data;
            displayGrades();
        });
}


async function getSubjects() {
    await fetch('http://localhost:4180/Education/Subjects')
        .then(response => response.json())
        .then(data => {
            subjects = data;
            generateSubjectOptions('gradeCreateSubject');
            generateSubjectOptions('gradeUpdateSubject');
        });
}


async function getTeachers() {
    await fetch('http://localhost:4180/People/Teachers')
        .then(response => response.json())
        .then(data => {
            teachers = data;
            generateTeacherOptions('gradeCreateTeacher');
            generateTeacherOptions('gradeUpdateTeacher');
        });
}

async function getStudents() {
    await fetch('http://localhost:4180/People/Students')
        .then(response => response.json())
        .then(data => {
            students = data;
            generateStudentOptions('gradeCreateStudent');
            generateStudentOptions('gradeUpdateStudent');
        });
}

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:4180/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("GradeCreated", (user, message) => {
        getgrades();
    });

    connection.on("GradeUpdated", (user, message) => {
        getgrades();
    });

    connection.on("GradeDeleted", (user, message) => {
        getgrades();
    });

    connection.onclose(async () => {
        await start();
    });
    start();

}

async function initialize() {
    await getStudents();
    await getTeachers();
    await getSubjects();
    getGrades();
    setupSignalR();
}


function cancelUpdate() {
    document.getElementById("gradeUpdateDiv").style.display = 'none';
    document.getElementById("gradeCreateDiv").style.display = 'block';
}

function displayErrorMessage(message) {
    document.getElementById("gradeAlertDiv").innerHTML =
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
    document.getElementById("gradeAlertDiv").innerHTML =
        `  
        <div class="alert alert-success alert-dismissible fade show" role="alert">
            <div>
                <i class="bi bi-check-circle mx-2"></i>
                <strong>${message}</strong>
            </div>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`
}
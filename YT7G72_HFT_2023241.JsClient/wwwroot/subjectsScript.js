let subjects = [];
let teachers = [];
let curriculums = [];
let requirements = [
    { ordinal: 0, value: 'Teachers Signature'},
    { ordinal: 1, value: 'Midterm Mark' },
    { ordinal: 2, value: 'Written Exam' },
    { ordinal: 3, value: 'Spoken Exam' },
    { ordinal: 4, value: 'Written and Spoken Exam' },
]


initialize();



function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:4180/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("SubjectCreated", (user, message) => {
        getSubjects();
    });

    connection.on("SubjectUpdated", (user, message) => {
        getSubjects();
    });

    connection.on("SubjectDeleted", (user, message) => {
        getSubjects();
    });

    connection.on("TeacherUpdated", async (user, message) => {
        await getTeachers();
        getSubjects();
    });

    connection.on("TeacherDeleted", async (user, message) => {
        await getTeachers();
        getSubjects();
    });

    connection.on("CurriculumUpdated", async (user, message) => {
        await getCurriculums();
        getSubjects();
    });

    connection.on("CurriculumDeleted", async (user, message) => {
        await getCurriculums();
        getSubjects();
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
            generateTeacherOptions("subjectCreateTeacher");
            generateTeacherOptions("subjectUpdateTeacher");
        });
}

function displaySubjects() {
    const subjectRows = subjects.map(s => `
        <tr>
            <td>${s.subjectId}</td>
            <td>${s.subjectName}</td>
            <td>${s.subjectCode}</td>
            <td>${s.credits}</td>
            <td>${convertTeacherToString(s.teacherId)}</td>
            <td>${convertRequirementToString(s.requirement)}</td>
            <td>${convertPreRequirementToString(s.preRequirementId)}</td>
            <td>${convertCurriculumToString(s.curriculumId)}</td>
            <td><button type="button" class="btn btn-info" onclick="displayUpdateSubject(${s.subjectId})">Update</button><button type="button" class="btn btn-danger" onclick="removeSubject(${s.subjectId})">Delete</button></td>
        </tr>`
    );
    document.getElementById('subjects').innerHTML = subjectRows.join('');
}


async function removeSubject(subjectId) {
    const deleteUrl = 'http://localhost:4180/Education/Subjects/' + subjectId;
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
            displaySuccessMessage("Subject deleted!");
            getSubjects();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

function resetCreationForm() {
    document.getElementById("subjectCreateSubjectName").value = '';
    document.getElementById("subjectCreateSubjectCode").value = '';
    document.getElementById("subjectCreateCredits").value = 0;
    document.getElementById("subjectCreateTeacher").value = document.getElementById("subjectCreateTeacher").children[0].value;
    document.getElementById("subjectCreatePreRequirement").value = null;
    document.getElementById("subjectCreateRequirement").value = document.getElementById("subjectCreateRequirement").children[0].value;
    document.getElementById("subjectCreateCurriculum").value = document.getElementById("subjectCreateCurriculum").children[0].value;
}

async function updateSubject(subjectId) {
    const updateUrl = 'http://localhost:4180/Education/Subjects'
    let options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            SubjectId: Number(subjectId),
            SubjectName: document.getElementById("subjectUpdateSubjectName").value,
            SubjectCode: document.getElementById("subjectUpdateSubjectCode").value,
            Credits: Number(document.getElementById("subjectUpdateCredits").value),
            TeacherId: Number(document.getElementById("subjectUpdateTeacher").value),
            PreRequirementId: Number(document.getElementById("subjectUpdatePreRequirement").value),
            Requirement: Number(document.getElementById("subjectUpdateRequirement").value),
            curriculumId: Number(document.getElementById("subjectUpdateCurriculum").value)
        })
    };


    try {
        const response = await fetch(updateUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Subject updated!");
            document.getElementById("subjectCreateDiv").style.display = 'block';
            document.getElementById("subjectUpdateDiv").style.display = 'none';
            getSubjects();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

async function createSubject() {
    const createUrl = 'http://localhost:4180/Education/Subjects';
    let options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            SubjectName: document.getElementById("subjectCreateSubjectName").value,
            SubjectCode: document.getElementById("subjectCreateSubjectCode").value,
            Credits: Number(document.getElementById("subjectCreateCredits").value),
            TeacherId: Number(document.getElementById("subjectCreateTeacher").value),
            PreRequirementId: Number(document.getElementById("subjectCreatePreRequirement").value),
            Requirement: Number(document.getElementById("subjectCreateRequirement").value),
            curriculumId: Number(document.getElementById("subjectCreateCurriculum").value)
        })
    };

    try {
        const response = await fetch(createUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Subject created!");
            resetCreationForm();
            getSubjects();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}


function displayUpdateSubject(subjectId) {
    let subject = subjects.find(s => s.subjectId == subjectId);
    document.getElementById("subjectUpdateId").value = subjectId;
    document.getElementById("subjectUpdateSubjectName").value = subject?.subjectName;
    document.getElementById("subjectUpdateSubjectCode").value = subject?.subjectCode;
    document.getElementById("subjectUpdateCredits").value = subject?.credits;
    document.getElementById("subjectUpdateTeacher").value = subject?.teacherId != null ? subject.teacherId : null;
    generatePreRequirementOptions("subjectUpdatePreRequirement", true);
    document.getElementById("subjectUpdateRequirement").value = subject?.requirement;
    document.getElementById("subjectUpdatePreRequirement").value = subject?.preRequirement != null ? subject.preRequirement : null;
    document.getElementById("subjectUpdateCurriculum").value = subject?.curriculumId;
    document.getElementById("subjectUpdateButton").addEventListener("click", () => updateSubject(subjectId));
    document.getElementById("subjectCreateDiv").style.display = 'none';
    document.getElementById("subjectUpdateDiv").style.display = 'block';

}

function cancelUpdate() {
    document.getElementById("subjectUpdateDiv").style.display = 'none';
    document.getElementById("subjectCreateDiv").style.display = 'block';
}

function displayErrorMessage(message) {
    document.getElementById("subjectAlertDiv").innerHTML =
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
    document.getElementById("subjectAlertDiv").innerHTML =
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

async function initialize() {
    await getCurriculums();
    await getTeachers();
    getSubjects();
    setupSignalR();
}

async function getCurriculums() {
    await fetch('http://localhost:4180/Curriculums')
        .then(response => response.json())
        .then(data => {
            curriculums = data;
            generateCurriculumOptions("subjectCreateCurriculum");
            generateCurriculumOptions("subjectUpdateCurriculum");
        });
}

async function getSubjects() {
    await fetch('http://localhost:4180/Education/Subjects')
        .then(response => response.json())
        .then(data => {
            subjects = data;
            displaySubjects();
            generatePreRequirementOptions("subjectCreatePreRequirement");
            generateRequirementOptions("subjectCreateRequirement");
            generateRequirementOptions("subjectUpdateRequirement");
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

function generatePreRequirementOptions(selectId, isUpdating = false) {
    let option = document.getElementById(selectId)
    if (isUpdating) {
        subjects.forEach((s) => {
            if (s.subjectId != document.getElementById("subjectUpdateId").value) {
                var newChild = document.createElement("option");
                newChild.setAttribute("value", s.subjectId);
                newChild.innerText = s.subjectName;
                option.appendChild(newChild);
            }
        })
    } else {
        subjects.forEach((s) => {
            var newChild = document.createElement("option");
            newChild.setAttribute("value", s.subjectId);
            newChild.innerText = s.subjectName;
            option.appendChild(newChild);
        });
    }
}

function generateRequirementOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    requirements.forEach((obj) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", obj.ordinal);
        newChild.innerText = obj.value;
        option.appendChild(newChild);
    });
}

function generateTeacherOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    let noTeacher = document.createElement("option");
    noTeacher.setAttribute("value", null);
    noTeacher.innerText = 'None';
    option.appendChild(noTeacher);
    teachers.forEach((t) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", t.teacherId);
        newChild.innerText = t.firstName + ' ' + t.lastName;
        option.appendChild(newChild);
    });
}

function convertTeacherToString(teacherId) {
    let foundTeacher = teachers.find(t => t.teacherId == teacherId);
    if (foundTeacher != null)
        return foundTeacher.firstName + ' ' + foundTeacher.lastName;
    return "";
}

function convertRequirementToString(requirement) {
    for (let obj of requirements) {
        if (obj.ordinal == requirement)
            return obj.value;
    }

    return "Unknown";
}

function convertPreRequirementToString(preRequirementId) {
    const preRequirement = subjects.find(s => s.subjectId == preRequirementId);
    if (preRequirement != null)
        return preRequirement.subjectName;
    return "";
}

function convertCurriculumToString(curriculumId) {
    const foundCurriculum = curriculums.find(c => c.curriculumId == curriculumId);
    if (foundCurriculum != null)
        return foundCurriculum.curriculumName;
    return "";
}

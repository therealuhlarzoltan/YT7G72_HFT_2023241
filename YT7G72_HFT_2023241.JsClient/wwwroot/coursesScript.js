let daysOfWeek = [
    { ordinal: 0, value: 'Sunday' },
    { ordinal: 1, value: 'Monday' },
    { ordinal: 2, value: 'Tuesday' },
    { ordinal: 3, value: 'Wedensday' },
    { ordinal: 4, value: 'Thursday' },
    { ordinal: 5, value: 'Friday' },
    { ordinal: 6, value: 'Saturday' }
];

let courseTypes = [
    { ordinal: 0, value: 'Lecture' },
    { ordinal: 1, value: 'ELearning' },
    { ordinal: 2, value: 'Tutorial' },
    { ordinal: 3, value: 'Lab' }
];

let teachers = [];
let subjects = [];
let courses = [];
let students = [];

initialize();

async function updateCourse(courseId) {
    const updateUrl = 'http://localhost:4180/Education/Courses'
    let options = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            CourseId: Number(courseId),
            CourseName: document.getElementById("courseUpdateCourseName").value,
            CourseCapacity: document.getElementById("courseUpdateCapacity").value,
            Room: document.getElementById("courseUpdateRoom").value,
            SubjectId: Number(document.getElementById("courseUpdateSubject").value),
            TeacherId: Number(document.getElementById("courseUpdateTeacher").value),
            LengthInMinutes: Number(document.getElementById("courseUpdateLength").value),
            StartTime: document.getElementById("courseUpdateStartTime").value,
            DayOfWeek: Number(document.getElementById("courseUpdateDay").value),
            CourseType: Number(document.getElementById("courseUpdateCourseType").value)
        })
    };


    try {
        const response = await fetch(updateUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Course updated!");
            document.getElementById("courseCreateDiv").style.display = 'block';
            document.getElementById("courseUpdateDiv").style.display = 'none';
            getCourses();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

async function createCourse() {
    const createUrl = 'http://localhost:4180/Education/Courses';
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            CourseName: document.getElementById("courseCreateCourseName").value,
            CourseCapacity: document.getElementById("courseCreateCapacity").value,
            Room: document.getElementById("courseCreateRoom").value,
            SubjectId: Number(document.getElementById("courseCreateSubject").value),
            TeacherId: Number(document.getElementById("courseCreateTeacher").value),
            LengthInMinutes: Number(document.getElementById("courseCreateLength").value),
            StartTime: document.getElementById("courseCreateStartTime").value,
            DayOfWeek: Number(document.getElementById("courseCreateDay").value),
            CourseType: Number(document.getElementById("courseCreateCourseType").value)
        })
    };


    try {
        const response = await fetch(createUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Course created!");
            resetCreationForm();
            getCourses();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}

async function removeCourse(courseId) {
    const deleteUrl = 'http://localhost:4180/Education/Courses/' + courseId;
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
            displaySuccessMessage("Course deleted!");
            getCourses();
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }

}

function convertCourseTypeToString(courseType) {
    const foundCourseType = courseTypes.find(ct => ct.ordinal == courseType);
    if (foundCourseType != null)
        return foundCourseType.value;
    return "Unknown";
}

function convertDayOfWeekToString(dayOfWeek) {
    const foundDay = daysOfWeek.find(d => d.ordinal == dayOfWeek);
    if (foundDay != null)
        return foundDay.value;
    return "Unknown";
}

function convertTeacherToString(teacherId) {
    const foundTeacher = teachers.find(t => t.teacherId == teacherId);
    if (foundTeacher != null)
        return foundTeacher.firstName + ' ' + foundTeacher.lastName;
    return "";
}

function convertSubjectToString(subjectId) {
    const foundSubject = subjects.find(s => s.subjectId == subjectId);
    if (foundSubject != null)
        return foundSubject.subjectName;
    return "Unknown";
}

function displayCourses() {
    const courseRows = courses.map(c => `
        <tr>
            <td>${c.courseId}</td>
            <td>${c.courseName}</td>
            <td>${c.numberOfRegistrations}/${c.courseCapacity}</td>
            <td>${convertCourseTypeToString(c.courseType)}</td>
            <td>${convertDayOfWeekToString(c.dayOfWeek)}</td>
            <td>${c.startTime}</td>
            <td>${c.room}</td>
            <td>${c.lengthInMinutes}</td>
            <td>${convertTeacherToString(c.teacherId)}</td>
            <td>${convertSubjectToString(c.subjectId)}</td>
            <td><button type="button" class="btn btn-info" onclick="displayUpdateCourse(${c.courseId})">Update</button><button type="button" class="btn btn-danger" onclick="removeCourse(${c.courseId})">Delete</button></td>
        </tr>`
    );
    document.getElementById('courses').innerHTML = courseRows.join('');
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

function generateCourseOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    courses.forEach((c) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", c.courseId);
        newChild.innerText = c.courseName;
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

function generateTeacherOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '<option value="null">None</option>';
    teachers.forEach((t) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", t.teacherId);
        newChild.innerText = t.firstName + ' ' + t.lastName;
        option.appendChild(newChild);
    });
}

function generateDayOfWeekOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    daysOfWeek.forEach((obj) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", obj.ordinal);
        newChild.innerText = obj.value;
        option.appendChild(newChild);
    });
}


function generateCourseTypeOptions(selectId) {
    let option = document.getElementById(selectId)
    option.innerHTML = '';
    courseTypes.forEach((obj) => {
        var newChild = document.createElement("option");
        newChild.setAttribute("value", obj.ordinal);
        newChild.innerText = obj.value;
        option.appendChild(newChild);
    });
}

function resetCreationForm() {
    document.getElementById("courseCreateCourseName").value = '';
    document.getElementById("courseCreateCapacity").value = 0;
    document.getElementById("courseCreateRoom").value = '';
    document.getElementById("courseCreateCourseType").value = 0;
    document.getElementById("courseCreateDay").value = 0;
    document.getElementById("courseCreateStartTime").value = '';
    document.getElementById("courseCreateLength").value = 0;
    document.getElementById("courseCreateTeacher").value = 'null';
    document.getElementById("courseCreateSubject").value = document.getElementById("courseCreateSubject").children[0].value;


}

function displayUpdateCourse(courseId) {
    const course = courses.find(c => c.courseId == courseId);
    document.getElementById("courseUpdateId").value = courseId;
    document.getElementById("courseUpdateCourseName").value = course?.courseName;
    document.getElementById("courseUpdateCapacity").value = course?.courseCapacity;
    document.getElementById("courseUpdateRoom").value = course?.room;
    document.getElementById("courseUpdateCourseType").value = course?.courseType;
    document.getElementById("courseUpdateDay").value = course?.dayOfWeek;
    document.getElementById("courseUpdateStartTime").value = course?.startTime;
    document.getElementById("courseUpdateLength").value = course?.lengthInMinutes;
    document.getElementById("courseUpdateTeacher").value = course?.teacherId != null ? course.teacherId : 'null';
    document.getElementById("courseUpdateSubject").value = course?.subjectId;
    document.getElementById("courseCreateDiv").style.display = 'none';
    document.getElementById("courseUpdateDiv").style.display = 'block';
    document.getElementById("courseUpdateButton").addEventListener("click", () => updateCourse(courseId));

}

async function getCourses() {
    await fetch('http://localhost:4180/Education/Courses')
        .then(response => response.json())
        .then(data => {
            courses = data;
            displayCourses();
            generateDayOfWeekOptions("courseCreateDay");
            generateDayOfWeekOptions("courseUpdateDay");
            generateCourseTypeOptions("courseCreateCourseType");
            generateCourseTypeOptions("courseUpdateCourseType");
            generateCourseOptions('registrationCourse');
        });
}


async function getSubjects() {
    await fetch('http://localhost:4180/Education/Subjects')
        .then(response => response.json())
        .then(data => {
            subjects = data;
            generateSubjectOptions('courseCreateSubject');
            generateSubjectOptions('courseUpdateSubject');
            generateSubjectOptions('registrationSubject');

        });
}


async function getTeachers() {
    await fetch('http://localhost:4180/People/Teachers')
        .then(response => response.json())
        .then(data => {
            teachers = data;
            generateTeacherOptions("courseCreateTeacher");
            generateTeacherOptions("courseUpdateTeacher");
        });
}


async function getStudents() {
    await fetch('http://localhost:4180/People/Students')
        .then(response => response.json())
        .then(data => {
            students = data;
            generateStudentOptions("registrationSubjectStudent");
            generateStudentOptions("registrationCourseStudent");
            
        });
}


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:4180/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("CourseCreated", (user, message) => {
        getCourses();
    });

    connection.on("CourseUpdated", (user, message) => {
        getCourses();
    });

    connection.on("CourseDeleted", (user, message) => {
        getCourses();
    });

    connection.on("SubjectUpdated", async (user, message) => {
        await getSubjects();
        getCourses();
    });

    connection.on("SubjectCreated", async (user, message) => {
        await getSubjects();
        getCourses();
    });


    connection.on("SubjectDeleted", async (user, message) => {
        await getSubjects();
        getCourses();
    });

    connection.on("TeacherUpdated", async (user, message) => {
        await getTeachers();
        getCourses();
    });

    connection.on("TeacherCreated", async (user, message) => {
        await getTeachers();
        getCourses();
    });

    connection.on("TeacherDeleted", async (user, message) => {
        await getTeachers();
        getCourses();
    });

    connection.on("StudentUpdated", async (user, message) => {
        await getStudents();
        getCourses();
    });

    connection.on("StudentDeleted", async (user, message) => {
        await getStudents();
        getCourses();
    });

    connection.on("StudentCreated", async (user, message) => {
        await getStudents();
        getCourses();
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

async function initialize() {
    await getTeachers();
    await getSubjects();
    await getStudents();
    getCourses();
    setupSignalR();
}


function cancelUpdate() {
    document.getElementById("courseUpdateDiv").style.display = 'none';
    document.getElementById("courseCreateDiv").style.display = 'block';
}

function displayErrorMessage(message) {
    document.getElementById("courseAlertDiv").innerHTML =
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
    document.getElementById("courseAlertDiv").innerHTML =
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
async function registerForCourse() {

    const courseId = document.getElementById("registrationCourse").value;
    const studentId = document.getElementById("registrationCourseStudent").value;;
    const registrationUrl = `http://localhost:4180/Education/Courses/${courseId}/Register/${studentId}`;
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    };
    try {
        const response = await fetch(registrationUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Successfully registered for course!");
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}

async function registerForSubject() {

    const subjectId = document.getElementById("registrationSubject").value;
    const studentId = document.getElementById("registrationSubjectStudent").value;;
    const registrationUrl = `http://localhost:4180/Education/Subjects/${subjectId}/Register/${studentId}`;
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    };
    try {
        const response = await fetch(registrationUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Successfully registered for subject!");
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}

async function unregisterFromSubject() {
    const subjectId = document.getElementById("registrationSubject").value;
    const studentId = document.getElementById("registrationSubjectStudent").value;;
    const registrationUrl = `http://localhost:4180/Education/Subjects/${subjectId}/Register/${studentId}`;
    const options = {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    };
    try {
        const response = await fetch(registrationUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Successfully unregistered from subject!");
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}

async function unregisterFromCourse() {
    const courseId = document.getElementById("registrationCourse").value;
    const studentId = document.getElementById("registrationCourseStudent").value;;
    const registrationUrl = `http://localhost:4180/Education/Courses/${courseId}/Register/${studentId}`;
    const options = {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    };
    try {
        const response = await fetch(registrationUrl, options);
        if (!response.ok) {
            const data = await response.json();
            displayErrorMessage(data.msg != null ? data.msg : data['errors'][Object.keys(data['errors'])[0]][0]);
        } else {
            displaySuccessMessage("Successfully unregistered from course!");
        }
    } catch (error) {
        displayErrorMessage("Something went wrong...");
        console.error('Error:', error);
    }
}


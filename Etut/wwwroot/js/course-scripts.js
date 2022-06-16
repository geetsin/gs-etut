// JavaScript code related to courses

$(document).ready(function () {


});

var routeURL = location.protocol + "//" + location.host;

function submitCourse() {
    var videoListData = gatherAllVideoElements();
    var requestData = {
        Name: $("#inputCourseName").val(),
        Description: $("#inputCourseDescription").val(),
        VideoList: videoListData,
    };
    console.log("LOG: reqData: " + JSON.stringify(requestData));
    const apiURL = routeURL + '/api/courses/create';
    console.log("LOG: API URL: " + apiURL);
    $.ajax({
        url: apiURL,
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (response) {
            if (response.status != 0) {
                alert(response.message);
                window.location.href = "/course/";
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            console.log("LOG: **ERROR** -- ", xhr);
            alert("Error");
        }
    });
}

function addNewVideoChapter() {
    var videoList = document.getElementById("video-list");
    var newVideoCount = videoList.getElementsByClassName("video-chapter").length + 1;

    var newChapterLabelElement = `<label class="form-label mt-2" for="inputChapterName">Chapter ${newVideoCount}</label>`;
    var newChapterTitleInputElement = `<input type="text"class="form-control" id="inputChapterName" placeholder="Chapter ${newVideoCount} Title">`;
    var newChapterDescriptionTextAreaElement = `<textarea class="form-control mt-1" id="inputChapterDescription" rows="2" placeholder="Chapter ${newVideoCount} description"></textarea>`;
    var newChapterVideoUrlElement = `<input class="form-control mt-2" type="text" class="form-control" id="inputVideoUrl" placeholder="Chapter ${newVideoCount} Video URL">`;

    var newVideoElementDiv = document.createElement("div");
    newVideoElementDiv.classList.add("video-chapter");
    newVideoElementDiv.innerHTML = newChapterLabelElement + newChapterTitleInputElement + newChapterDescriptionTextAreaElement + newChapterVideoUrlElement;

    if (newVideoCount == 10) {
        document.getElementById("addNewChapterBtn").disabled = true;
        document.getElementById("addNewChapterBtn").innerText = "*You can add only upto 10 chapters per course";
    }
    videoList.appendChild(newVideoElementDiv);

}

function gatherAllVideoElements() {
    var videoListElement = document.getElementById("video-list");
    var videoElementsCollection = videoListElement.getElementsByClassName("video-chapter");

    var videosList = [];

    Array.from(videoElementsCollection).forEach(
        function (videoElement, index, array) {
            // do stuff
            var videoName = $(videoElement).find('input[id="inputChapterName"]').val();
            var videoDescription = $(videoElement).find('textarea[id="inputChapterDescription"]').val();
            var videoUrl = $(videoElement).find('input[id="inputVideoUrl"]').val();

            var videoChapter = {
                Name: videoName,
                Description: videoDescription,
                VideoUrl: videoUrl,
            };
            videosList[index] = videoChapter;
        }
    );

    return videosList;
}


///////

//Scripts for Course Deatails Page

///////


function showMeThisChapter(pClickedCard) {
    let videoName = pClickedCard.querySelector('.card-title');
    let videoChapterNo = pClickedCard.querySelector('#video-side-chapter');
    let videoDescription = pClickedCard.querySelector('#video-side-description');
    let videoURL = videoName.getAttribute('data-url');

    document.getElementById("video-area-chapter-name").innerHTML = videoName.innerText;
    document.getElementById("video-area-chapter-number").innerHTML = videoChapterNo.innerText;
    document.getElementById("video-area-description").innerHTML = videoDescription.textContent;
    document.getElementById("video-area-url").setAttribute("src", videoURL);
}

function getStudentsList() {
    const apiURL = routeURL + '/api/users/students/' + $("#my-courses-select").val();
    console.log("LOG: API URL: " + apiURL);

    $.ajax({
        url: apiURL,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status == 1 && response.dataenum != undefined) {
                checkStudentsEnrolledChecbox(response.dataenum);
            }
        },
        error: function (xhr) {
            console.log("**LOG: ## API Call ERROR## = ", xhr);
        }
    });

}

function checkStudentsEnrolledChecbox(pStudentList) {
    clearStudentListCheckboxInputs();

    for (let student of pStudentList) {
        $(".input-group-text input[type=checkbox]").each(function () {

            var asdf = $(this).attr('data-id');
            if ($(this).attr('data-id') == student.id) {
                $(this).prop("checked", true);
                $(this).prop('disabled', true);
                $(this).attr('data-course-assigned', "true");
            }

        });

    }
}

function clearStudentListCheckboxInputs() {
    $(".input-group-text input[type=checkbox]").each(function () {
        $(this).prop("checked", false);
        $(this).prop('disabled', false);
        $(this).attr('data-course-assigned', "false");


    });
}

function addUsersToCourse() {
    var reqData = []; // The array with UserCourse objects
    $('.input-group-text input:checked').each(function () {
        if ($(this).attr("data-course-assigned") == "false") {
            reqData.push({
                Id: "dummyID", // Just so that the API call satisfies UserCourse.cs class
                CourseId: $("#my-courses-select").val(),
                UserId: $(this).attr('data-id')
            });
        }

    });
    if ($("#my-courses-select").val() == null) {
        alert("Please select a course");
    } else if (reqData.length == 0) {
        alert("No users are selected");
    } else {
        // Yes, a course is selected and a new user is selected.
        

        const apiURL = routeURL + '/api/courses/assign-course';
        console.log("LOG: API URL: " + apiURL);
        $.ajax({
            url: apiURL,
            type: 'POST',
            data: JSON.stringify(reqData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status == 1) {
                    alert(response.message);
                    window.location.reload();
                } else {
                    alert(response.message);
                }
                getAppointments();
                onCloseModal();
            },
            error: function (xhr) {
                console.log("LOG: **ERROR** -- ", xhr);
                alert("Error");
            }
        });

    }




}
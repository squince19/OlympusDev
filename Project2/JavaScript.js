function LogOn(username, userPassword) {
 
    var webMethod = "WebService.asmx/LogOn";
    var parameters = "{\"username\":\"" + encodeURI(username) + "\",\"userPassword\":\"" + encodeURI(userPassword) + "\"}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            if (msg.d) {
                window.location.href = 'EmployeeData.html';

                
            }
            else {
                alert("Login Failed. Wrong username or password")
            }
        },
        error: function (e) {
            alert("boo...");
        }
    });
}

function GetInfo(employeeID) {
    var webMethod = "WebService.asmx/GetEmployeeInformation";
    var parameters = "{\"employeeID\":\"" + encodeURI(employeeID) + "\"}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data:parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            if (msg.d) {
                document.getElementById('employeeName').innerHTML = msg.d.fname + " " + msg.d.lname;
            }
            else {
                alert("FAIL :(")
            }
        },
        error: function (e) {
            alert("boo...");
        }
    });
}
function hello() {
    console.log("hey there");
}

var employeeArray;


//function in progress SQ
function LoadEmployees() {
    var webMethod = "WebService.asmx/GetNames";
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            alert("success");
            if (msg.d) {

                employeeArray = msg.d;
                for (i = 0; i < employeeArray.length; i++) {
                    console.log(employeeArray[i].employeeId);
                    var scrollBar = document.getElementById('employeeScrollBar');
                    var pNode = document.createElement('p');
                    var value = employeeArray[i].employeeId;
                    console.log(value);
                    pNode.setAttribute("id", value);
                    pNode.setAttribute("onclick", GetInfo(value));
                    pValue = employeeArray[i].fname + " " + employeeArray[i].lname;
                    pNode.innerHTML = pValue;
                    scrollBar.appendChild(pNode);
                    
                }
            }
            else {
                alert("Login Failed. Wrong username or password")
            }
        },
        error: function (e) {
            alert("boo...");
        }
    });
    
}





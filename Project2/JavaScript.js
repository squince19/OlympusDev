sessionStorage.setItem('logOn', 'false');
var log = sessionStorage.getItem('logOn');
log = false;

function LogOn(username, userPassword) {
 
    var webMethod = "WebService.asmx/LogOn";
    var parameters = "{\"username\":\"" + encodeURI(username) + "\",\"userPassword\":\"" + encodeURI(userPassword) + "\"}";
    var LogOn = sessionStorage.getItem('LogOn');

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
                sessionStorage.setItem('logOn', 'true');
                log = sessionStorage.getItem('logOn');


                sessionStorage.setItem('LogOn') = true;
            }
            else {
                alert("Login Failed. Wrong username or password")
                console.log(test);
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

//Test function
//function hello() {
//    console.log("hey there");
//}

var employeeArray;


function LoadEmployees(log) {
    var webMethod = "WebService.asmx/GetNames";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: log,
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
                    pNode.setAttribute("onclick", "GetInfo(" + value + ")");
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

//fucntion for EmployeeGraph
var tableArr;
function LoadChart() {
    var webMethod = "WebService.asmx/EmployeeGraph";
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            if (msg.d.length > 0) {
                tableArr = msg.d;
                var table = document.createElement('table');
                table.id = 'prodTable';
                alert("Table created")
                for (var i = 0; i < tableArr.length; i++) {
                    var row = document.createElement('tr');
                    row.setAttribute("id", tableArr[0].employeeID);
                    for (var j = 0; j <= 5; j++) {
                        var cell = document.createElement('td');
                        if (j == 0) {
                            cell.textContent = tableArr[i].lname + ", " + tableArr[i].fname;
                            cell.setAttribute("id", "empName");
                        }
                        else if (j > 0) {
                            cell.setAttribute("id", "empData");
                            if (tableArr[i].productivityLevel[j - 1] >= 80) {
                                cell.bgColor = "Green";
                            }
                            else if (tableArr[i].productivityLevel[j - 1] < 80 && tableArr[i].productivityLevel[j - 1] >= 65) {
                                cell.bgColor = "Yellow";
                            }
                            else if (tableArr[i].productivityLevel[j - 1] < 65) {
                                cell.bgColor = "Red";
                            }
                        }
                        row.appendChild(cell);
                    }
                    table.appendChild(row);
                }
                document.getElementById('productivityTable').appendChild(table);
            }
            else {
                alert("Error occurred loading graph");
            }
        },
        error: function (e) {
            alert("Error occurred communicating with server");
        }
    });
}






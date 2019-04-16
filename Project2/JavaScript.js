
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

                window.localStorage.setItem('LogOn', 'true');
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

function GetName() {
    webMethod = "WebService.asmx/ManagerName";
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8:",
        dataType: "json",
        success: function (msg) {
            var name = msg.d;
            window.localStorage.setItem('ManagerName', name);

        }
    })
}
var x = 0;

function GetNotes(employeeID) {
    var webMethod = "WebService.asmx/LoadNotes";
    var parameters = "{\"employeeID\":\"" + encodeURI(employeeID) + "\"}";
    var noteArray;
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8:",
        dataType: "json",
        success: function(msg) {
                if (msg.d) {
                    noteArray = msg.d;
                    for (i = 0; i < noteArray.length; i++) {

                        console.log(noteArray[i].EmployeeID);
                        var scrollBar = document.getElementById('notesScrollBar');
                        var pNode = document.createElement('p');

                        var value = noteArray[i].Subject;            
                        console.log(value);
                        pNode.setAttribute("id", "note" + noteArray[i].NoteID);
                        pNode.setAttribute("class", "noteClass");
                        pNode.setAttribute("onclick", "DisplayNoteInfo(" + noteArray[i].NoteID + ")");
                        pNode.innerHTML = value;
                        scrollBar.appendChild(pNode);
            }
        }
        else {
            alert("FAIL :(");
        }
    
        },
        error: function (e) {
            alert("boo...");
        }
    });

}

function DisplayNoteInfo(id) {
    var webMethod = "WebService.asmx/GetNoteInfo";
    var parameters = "{\"id\":\"" + encodeURI(id) + "\"}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8:",
        dataType: "json",
        success: function (msg) {
            if (msg.d) {
                var note = msg.d;
                document.getElementById('addNotes').style.display = 'block';
                document.getElementById('modalHeader').innerHTML = "Note Information";
                document.getElementById('createBtn').style.display = 'none';
                document.getElementById('removeBtn').style.display = 'inline';
                document.getElementById('saveBtn').style.display = 'none';
                document.getElementById('editBtn').style.display = 'inline';

                document.getElementById('mgrIdTextBox').value = note.ManagerID;
                document.getElementById('subjectTextBox').value = note.Subject;
                document.getElementById('bodyTextBox').value = note.Body;

                document.getElementById('mgrIdTextBox').disabled = true;
                document.getElementById('subjectTextBox').disabled = true;
                document.getElementById('bodyTextBox').disabled = true;

                
            }
            
            else {
                alert("FAIL :(");
            }

        },
        error: function (e) {
            alert("boo...");
        }
    });

}//END DisplayNoteInfo

function GetInfo(employeeID) {
    window.localStorage.removeItem('empid');
    window.localStorage.setItem('empid', employeeID);
    alert(window.localStorage.getItem('empid'));
    var paras = document.getElementsByClassName('noteClass');
    while (paras[0]) {
        paras[0].parentNode.removeChild(paras[0]);
        
    }
    window.localStorage.removeItem('localEmployee');
    window.localStorage.setItem('localEmployee', employeeID);
    console.log(window.localStorage.getItem('localEmployee'));
    var webMethod = "WebService.asmx/GetEmployeeInformation";
    var parameters = "{\"employeeID\":\"" + encodeURI(employeeID) + "\"}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            if (msg.d) {
                document.getElementById('employeeName').innerHTML = msg.d.fname + " " + msg.d.lname;
                document.getElementById('deptName').innerHTML = msg.d.Department;
                GetNotes(employeeID);

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


var employeeArray;

function LoadAllEmployees() {
    LoadChart(false);
    var paras = document.getElementsByClassName('empClass');
    while (paras[0]) {
        paras[0].parentNode.removeChild(paras[0]);
    }

    var webMethod = "WebService.asmx/GetAllNames";
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d) {
                GetName();
                var nameInsert = window.localStorage.getItem('ManagerName');
                var greeting = "Welcome, " + nameInsert + "!";
                document.getElementById('welcomeGreeting').innerHTML = greeting;
                console.log(nameInsert);
                employeeArray = msg.d;
                for (i = 0; i < employeeArray.length; i++) {
                    console.log(employeeArray[i].employeeId);
                    var scrollBar = document.getElementById('employeeScrollBar');
                    var pNode = document.createElement('p');
                    var value = employeeArray[i].employeeId;
                    console.log(value);
                    pNode.setAttribute("id", value);
                    pNode.setAttribute("onclick", "GetInfo(" + value + ")");
                    pNode.setAttribute("class", "empClass");
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

//loads all employees in left side bar
function LoadEmployees() {

    LoadChart(true);
    var paras = document.getElementsByClassName('empClass');
    while (paras[0]) {
        paras[0].parentNode.removeChild(paras[0]);

    }

    var webMethod = "WebService.asmx/GetNames";
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d) {
                GetName();
                var nameInsert = window.localStorage.getItem('ManagerName');
                var greeting = "Welcome, " + nameInsert + "!";
                document.getElementById('welcomeGreeting').innerHTML = greeting;
                console.log(nameInsert);
                employeeArray = msg.d;
                for (i = 0; i < employeeArray.length; i++) {
                    console.log(employeeArray[i].employeeId);
                    var scrollBar = document.getElementById('employeeScrollBar');
                    var pNode = document.createElement('p');
                    var value = employeeArray[i].employeeId;
                    console.log(value);
                    pNode.setAttribute("id", value);
                    pNode.setAttribute("onclick", "GetInfo(" + value + ")");
                    pNode.setAttribute("class", "empClass");
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
function LoadChart(letters) {
    var table1 = document.getElementById('productivityTable');
    clearResultsTable(table1);

    var webMethod = "WebService.asmx/EmployeeGraph";
    
    var parameters = "{\"truefalse\":\"" + encodeURI(letters) + "\"}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            if (msg.d.length > 0) {
                tableArr = msg.d;
                var table = document.getElementById('productivityTable');
                for (var i = 0; i < tableArr.length; i++) {
                    var row = document.createElement('tr');
                    row.setAttribute("id", tableArr[0].employeeID);
                    var j;
                    for (j = 0; j <= 5; j++) {
                        var cell = document.createElement('td');
                        if (j == 0) {
                            cell.textContent = tableArr[i].lname + ", " + tableArr[i].fname;
                            cell.setAttribute("id", "empName");
                        }
                        else if (j > 0) {
                            cell.setAttribute("id", "empData");
                            if (tableArr[i].productivityLevel[j - 1] >= 70) {
                                cell.textContent = tableArr[i].productivityLevel[j - 1];
                                cell.style.backgroundColor = "#CFFF9E";
                            }
                            else if (tableArr[i].productivityLevel[j - 1] < 70 && tableArr[i].productivityLevel[j - 1] >= 65) {
                                cell.textContent = tableArr[i].productivityLevel[j - 1];
                                cell.style.backgroundColor = "#FFED00"
                            }
                            else if (tableArr[i].productivityLevel[j - 1] < 65) {
                                cell.textContent = tableArr[i].productivityLevel[j - 1];
                                cell.style.backgroundColor = "#FF576D"
                            }
                        }
                        row.appendChild(cell);
                    }
                    table.appendChild(row);
                }
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

function clearResultsTable(table) {
    for (var i = table.rows.length; i > 1; i--) {
        table.deleteRow(i - 1);

    }
}

function SearchEmployee() {
    var webMethod = "WebService.asmx/SearchEmployee";
    var name = document.getElementById('searchBar').value;
    var parameters = "{\"name\":\"" + encodeURI(name) + "\"}";
    var searchArray;
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8:",
        dataType: "json",
        success: function (msg) {
            if (msg.d) {
                searchArray = msg.d;
                for (i = 0; i < searchArray.length; i++) {
                    console.log(searchArray[i].fname);
                }
            }

            else {
                alert("FAIL :(");
            }

        },
        error: function (e) {
            alert("boo...");
        }
    });
}

function CreateNote() {
    var webMethod = "WebService.asmx/CreateNote";
    var body = document.getElementById('bodyTextBox').value;
    var subject = document.getElementById('subjectTextBox').value;
    var mgrid = document.getElementById('mgrIdTextBox').value;
    var employeeid = window.localStorage.getItem('empid');

    var parameters = "{\"body\":\"" + encodeURI(body) + "\",\"subject\":\"" + encodeURI(subject) + "\",\"mgrid\":\"" + encodeURI(mgrid) + "\",\"employeeID\":\"" + encodeURI(employeeid) +"\"}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            if (msg.d) {
                document.getElementById('addNotes').style.display = 'none';
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

function CreateNoteModal() {
    document.getElementById('addNotes').style.display = 'block';
    document.getElementById('subjectTextBox').value = "";
    document.getElementById('bodyTextBox').value = "";

    document.getElementById('mgrIdTextBox').disabled = false;
    document.getElementById('subjectTextBox').disabled = false;
    document.getElementById('bodyTextBox').disabled = false;

    document.getElementById('modalHeader').innerHTML = "New Note";
    document.getElementById('createBtn').style.display = 'inline';
    document.getElementById('removeBtn').style.display = 'none';
    document.getElementById('saveBtn').style.display = 'none';
    document.getElementById('editBtn').style.display = 'none';

}







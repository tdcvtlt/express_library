var resCount = 2;
var aResIDs = new Array(1, 2);
var aResTops = new Array(200, 300);
var aResLefts = new Array(0, 0);
var aResWidth = new Array(25, 25);
var sDate = new Date();
var aRooms = null;
var aRoomType = null;
var aLastRoomListID = 0;
var aResHoldNames = null;
var ResHoldStart = 0;
var ResOOSStart = 0;
/* above are changed dynamically */

//var reqRooms = null; //new ajaxRequest();
//var reqRes = null; //new ajaxRequest();
//var reqUPRes = null; // new ajaxRequest();


var sModulePath = "modules/roommatrix.asp";
var bWaitingRooms = false;
var bWaitingReservations = false;
var maxTop = 0;
var rowHeight = 0;
var gridTop = '100px';
var gridLeft = '450px';

var iHeight = '20px';
var iWidth = '25px';
var iTop = '20px';
var iLeft = '20px';

var drag = false;
var drop = false;
var oRes = null;
var dragResID = 0;

var x = 0;
var y = 0;
var zindex = 0;
var indexFr = 0;
var posX = 0;
var posY = 0;

var rowstart = 0;
var colstart = 0;
var month = new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec');
var day = new Array('U', 'M', 'T', 'W', 'H', 'F', 'S');
var month1 = 0;
var month2 = 0;
var month1Cols = 0;
var bRoomSearch = false;
var bPopMenu = false;
var insertX = 0;
var insertY = 0;
var insertGrid = 0;

function clicktest(reservation, e, showDrag) {
    if (!drag) {
        if (!bPopMenu && !drop) {
            zindex++;
            if (!e) e = event;
            var m = document.getElementById('popmenu');
            m.style.top = parseInt(reservation.style.top) + (parseInt(reservation.style.height) / 2) + 'px'; //e.clientY + 'px';
            m.style.left = parseInt(reservation.style.left) - parseInt(m.style.width) + 'px'; //e.clientX - parseInt(m.style.width) + 'px';
            m.style.border = "thin solid black";
            m.style.zIndex = zindex;
            m.style.visibility = 'visible';
            document.getElementById('popedit').style.visibility = 'visible';
            document.getElementById('popdrag').style.visibility = (showDrag) ? 'visible' : 'hidden';
            oRes = reservation;
            x = parseInt(reservation.style.left); //e.clientX;
            y = parseInt(reservation.style.top) + (parseInt(reservation.style.height) / 2); //e.clientY;
            bPopMenu = true;
        } else {
            var m = document.getElementById('popmenu');
            m.style.visibility = 'hidden';
            document.getElementById('popedit').style.visibility = 'hidden';
            document.getElementById('popdrag').style.visibility = 'hidden';
            m = document.getElementById('insertmenu');
            m.style.visibility = 'hidden';
            bPopMenu = false;
            drop = false;
        }
    }
}

function Insert_Res() {
    document.getElementById('popedit').style.visibility = 'hidden';
    //alert('Insert Res at position (' + insertX + ', ' + insertY + ') over Grid block# ' + insertGrid);
    var row = (insertGrid - (insertGrid % 30)) / 30;
    var col = insertGrid - (row * 30);
    var insertDate = new Date(sDate);
    insertDate.setTime(insertDate.getTime() + ((col - 1) * 24 * 60 * 60 * 1000));
    var insertRoom = aRooms[row - 1];
    var sMonth = insertDate.getMonth() + 1;
    var sDay = insertDate.getDate();
    var sYear = insertDate.getFullYear();
    popitup2('insertreservations.asp?room=' + insertRoom + "&Date=" + sMonth + '/' + sDay + '/' + sYear + "&Row=" + row + "&Col=" + col + "&roomtype=" + aRoomType[row - 1]);
}

function ClickEmpty(id, e) {
    /*
    if (!bPopMenu && id>30){
    zindex++;
    if (!e) e=event;
    var m = document.getElementById('insertmenu');
    m.style.top = parseInt(document.getElementById(id).style.top) + (parseInt(document.getElementById(id).style.height)/2)+'px';//e.clientY + 'px';
    m.style.left = parseInt(document.getElementById(id).style.left) + (parseInt(document.getElementById(id).style.width)/2) + 'px';//e.clientX - parseInt(m.style.width) + 'px';
    //m.style.border = "thin solid black";
    m.style.zIndex = zindex;
    m.style.visibility = 'visible';
    insertX=parseInt(document.getElementById(id).style.left) + (parseInt(document.getElementById(id).style.width)/2);//e.clientX;
    insertY=parseInt(document.getElementById(id).style.top) + (parseInt(document.getElementById(id).style.height)/2);//e.clientY;
    insertGrid = id;
    bPopMenu = true;
    } else {
    var m = document.getElementById('insertmenu');
    m.style.visibility = 'hidden';
    m = document.getElementById('popmenu');
    m.style.visibility = 'hidden';
    document.getElementById('popedit').style.visibility='hidden';
    document.getElementById('popdrag').style.visibility='hidden';
    bPopMenu=false;
    }
    */
}

function Init() {
    Build_Grid();
    Fill_Dates();
}

function Build_Grid() {
    for (i = 1; i < 452; i += 30) {
        for (x = 0; x < 30; x++) {
            var tmp = document.getElementById(i + x);
            tmp.style.top = (i < 30) ? parseInt(gridTop) + parseInt(iTop) - (20) + 'px' : (parseInt(gridTop) + (parseInt(iTop) + (((i) / 30) * 20))) + 'px';
            tmp.style.left = ((i + x) % 30 == 0) ? (parseInt(gridLeft) + (parseInt(iWidth) * 30)) + 'px' : (parseInt(gridLeft) + (parseInt(iWidth) * ((i + x) % 30))) + 'px';
            tmp.style.height = iHeight;
            tmp.style.width = iWidth;
        }
    }
    var holderTop = tmp.style.top;
    var holderLeft = document.getElementById('1').style.left;
    tmp = document.getElementById('holder');
    tmp.style.top = parseInt(holderTop) + (4 * parseInt(iHeight));
    tmp.style.height = 4 * parseInt(iHeight);
    tmp.style.left = holderLeft;
    tmp.style.width = parseInt(document.getElementById('30').style.left) + parseInt(document.getElementById('30').style.width) - parseInt(tmp.style.left);
    tmp = document.getElementById('holderentries');
    tmp.style.left = '100px';
    tmp.style.top = parseInt(holderTop) + (4 * parseInt(iHeight));
}

function Arrange_Reservations() {
    if (resCount == 0) {

    } else if (resCount > 1) {
        for (i = 0; i < resCount; i++) {
            var tmp = document.getElementById('res' + i);
            tmp.style.left = aResLefts[i]; // parseInt(gridLeft) + 'px';

            tmp.style.top = aResTops[i];
            tmp.style.width = aResWidth[i];
        }
    } else {
        var tmp = document.getElementById('res0');
        tmp.style.left = aResLefts; // parseInt(gridLeft) + 'px';
        tmp.style.top = aResTops;
        tmp.style.width = aResWidth;
    }
    Hide_Show_Holders(null, true);
}

function Select(val) {
    var f = document.forms[0].holdselect;
    if (f.options.length > 0) {
        Hide_Show_Holders(f.options[f.selectedIndex].value, false);
    }
}

function Hide_Show_Holders(val, loading) {
    val == null ? val = -1 : null;
    var f = document.forms[0].holdselect;
    if (loading) {
        if (f.options.length) {
            for (i = f.options.length; i > -1; i--) {
                f.options[i] = null;
            }
        }
    }
    if (aResHoldNames.length) {
        if ((ResHoldStart >= 0 && aResHoldNames.length > 0)) {
            for (var i = 0; i < aResHoldNames.length; i++) {
                if (document.getElementById('res' + ((ResHoldStart * 1) + i))) {
                    document.getElementById('res' + ((ResHoldStart * 1) + i)).style.visibility = (val == ((ResHoldStart * 1) + i)) ? 'visible' : 'hidden';
                }
                if (loading) {
                    f.options[f.options.length] = new Option(aResHoldNames[i], ((ResHoldStart * 1) + i));
                }
            }
        }
    }
}


function Fill_Rooms() {
    for (i = 0; i < 15; i++) {
        var tmp = document.getElementById("room" + i);
        var type = document.getElementById("roomtype" + i);
        tmp.style.width = '100px';
        type.style.width = '100px';
        tmp.style.left = parseInt(gridLeft) - parseInt(tmp.style.width) - parseInt(type.style.width) + parseInt(iWidth) + 'px';
        type.style.left = parseInt(tmp.style.left) + parseInt(tmp.style.width) + 'px';
        tmp.style.top = parseInt(gridTop) + (parseInt(iTop) * (i + 2)) + 'px';
        type.style.top = tmp.style.top;
        tmp.innerHTML = aRooms[i];
        type.innerHTML = aRoomType[i];
    }
}

function Fill_Dates() {
    month1Cols = 0;
    month1 = 0;
    var y1 = '';
    var y2 = '';
    for (i = 0; i < 30; i++) {
        var tmpDate = new Date(sDate);
        tmpDate.setTime(tmpDate.getTime() + (i * 24 * 60 * 60 * 1000));
        (month1 == 0 && tmpDate.getMonth() != 0 && i == 0) ? month1 = tmpDate.getMonth() : null;
        (month1 == tmpDate.getMonth()) ? month1Cols++ : month2 = tmpDate.getMonth();
        tmp = document.getElementById(i + 1);
        tmp.innerHTML = day[tmpDate.getDay()] + "<br>" + tmpDate.getDate();
        tmp.style.fontSize = '8pt';
        tmp.style.textAlign = 'center';
        (month1 == tmpDate.getMonth()) ? y1 = tmpDate.getFullYear() : y2 = tmpDate.getFullYear();
    }
    //set month(s)
    var m = document.getElementById('mon1');
    m.style.left = parseInt(gridLeft) + parseInt(iWidth) + 'px';
    m.style.top = parseInt(gridTop) - (20) + 'px';
    m.style.width = month1Cols * parseInt(iWidth) + 'px';
    m.style.border = 'thin solid black';
    m.style.font.align = 'center';
    m.innerHTML = month[month1]; // + y1;
    (parseInt(m.style.width) > (parseInt(iWidth) * 3)) ? m.innerHTML += ' ' + y1 : null;
    var m2 = document.getElementById('mon2');
    if (month1Cols != 30) {
        m2.style.visibility = 'visible';
        m2.style.left = parseInt(m.style.left) + parseInt(m.style.width) + 'px';
        m2.style.top = m.style.top;
        m2.style.border = m.style.border;
        m2.style.textalign = 'center';
        m2.innerHTML = month[month2]; // + y2;
        m2.style.width = (30 - month1Cols) * parseInt(iWidth) + 'px';
        (parseInt(m2.style.width) > (parseInt(iWidth) * 3)) ? m2.innerHTML += ' ' + y2 : null;
    } else {
        m2.style.visibility = 'hidden';
    }
}

function initializeDrag(reservation, e) {
    /*dragResID = parseInt(reservation.title);
    if (!e) e=event;
    //oRes=reservation;
    //x=e.clientX;
    //y=e.clientY;
    tempx=parseInt(reservation.style.left);
    tempy=parseInt(reservation.style.top);
    document.onmousemove=dragRes;
    */
}

function dragRes(e) {
    /*
    if (!e) e=event;
    zindex++;
    drag=true;
    var dir = 0; //up=1 down = 0
    maxTop = parseInt(document.getElementById('1').style.top)+20;
    var maxLeft = parseInt(document.getElementById('1').style.left);
    var maxBottom = parseInt(document.getElementById('450').style.top) + parseInt(document.getElementById('450').style.height);
    var maxRight = parseInt(document.getElementById('450').style.left) + parseInt(document.getElementById('450').style.width);
    rowHeight = parseInt(document.getElementById('1').style.height);
    //set drag direction up or down
    dir = (posX>tempx+e.clientX-x)?1:0;
    posX = tempx+e.clientX-x;
    posY = tempy+e.clientY-y;
    //if moved along x-axis reset back to original x-axis to prevent date conflicts on reservations
    posX=(posX!=tempx)?tempx:posX;
    //set y-axis to snap down or up to next row
    //maxTop = Top of 1st row
    //posY = current y-axis position
    //rowHeight = height of each row
    //posY-maxTop = distance to top (need to snap up or down to nearest 20)
    var disTop = posY-maxTop;
    posY=(dir==1)?posY-(disTop % rowHeight):posY-(disTop % rowHeight);
    
    //Limit top of drag
    posY<=maxTop+rowHeight?posY=maxTop+rowHeight:null;
    
    //Limit bottom of drag
    var maxB = maxTop+(rowHeight*15);
    var tmp = document.getElementById('holder');
    //check to see if dragging into holder (top of holder=640px, height = 100px)
    if (posY>=parseInt(tmp.style.top) && posY <=parseInt(tmp.style.top) + parseInt(tmp.style.height) - parseInt(iHeight)){
    posY=posY;
    } else {
    posY>=maxB?posY=maxB:null;
    }
    
    oRes.style.zIndex=zindex;
    oRes.style.left=posX+'px';
    oRes.style.top=posY+'px';
 
    if (e.clientX>=document.body.clientWidth-10 || e.clientY>=document.body.clientHeight-5 || e.clientX==5 || e.clientY==5){ //outside available window
    indexTo=indexFr;
    dropRes(oRes);
    }
    else indexTo = indexFr;
    */
    return false;
}

function dropRes(reservation) {
    document.onmousemove = new Function("return false");
    if (!drag) return;
    drag = false;
    Check_Overlap(reservation);
    reservation.zindex = 1;
    drop = true;
    return;
}

function Get_Rooms() {
    var reqRooms = new ajaxRequest();
    reqRooms.setCallBackFunc(function () { Get_Rooms_Ans(reqRooms); });
    var params = "Function=Get_Rooms";
    params += "&RoomListID=" + aLastRoomListID;
    if (bRoomSearch == true) {
        params += "&SearchRoom=" + document.forms[0].roomfilter.value;
        bRoomSearch = false;
    }
    reqRooms.doPost(sModulePath, params);
    //document.forms[0].roomfilter.value = aLastRoomListID;
    bWaitingRooms = true;
    setPointer(1);
    //document.getElementById('message').innerHTML += '<br>Requesting Rooms:' + bWaitingRooms;
}

function Get_Rooms_Ans(reqRooms) {
    var tmp = reqRooms.responseText.split('|&&|');
    aRooms = tmp[0].split(",");
    aRoomType = tmp[1].split(",");
    aLastRoomListID = tmp[2];
    Get_Reservations();
    Fill_Rooms();
    bWaitingRooms = false;
    setPointer(0);
}

function Get_Reservations() {
    //alert(sDate);
    var reqRes = new ajaxRequest();
    document.getElementById('reservations').innerHTML = "";
    reqRes.setCallBackFunc(function () { Get_Reservations_Ans(reqRes); });
    document.forms[0].holdselect.selectedIndex = -1;
    var params = "Function=Get_Reservations";
    params += "&sDate=" + encodeURI((sDate.getMonth() + 1) + '/' + sDate.getDate() + '/' + sDate.getFullYear());
    //alert(sDate.getFullYear());
    params += "&Rooms=" + encodeURI(aRooms.join(','));
    params += "&gridtop=" + parseInt(gridTop);
    params += "&iheight=" + parseInt(iHeight);
    params += "&iwidth=" + parseInt(iWidth);
    params += "&gridLeft=" + parseInt(gridLeft);
    //alert(params);
    reqRes.doPost(sModulePath, params);
    bWaitingReservations = true;
    setPointer(1);
}

function Get_Reservations_Ans(reqRes) {
    var tmp = reqRes.responseText.split('|&&|');
    //alert(req.responseText);
    resCount = tmp[0];
    if (resCount > 1) {
        aResIDs = tmp[1].split(",");
        aResTops = tmp[2].split(",");
        aResLefts = tmp[3].split(",");
        aResWidth = tmp[5].split(",");
        aResHoldNames = tmp[7].split("|");
        ResHoldStart = tmp[6];
        ResOOSStart = tmp[8];
    } else {
        aResIDs = tmp[1];
        aResTops = tmp[2];
        aResLefts = tmp[3];
        aResWidth = tmp[5];
        aResHoldNames = tmp[7];
        ResHoldStart = tmp[6];
        ResOOSStart = tmp[8];
    }
    document.getElementById('reservations').innerHTML = tmp[4];
    bWaitingReservations = false;
    Arrange_Reservations();
    setPointer(0);
    Get_Usages();
}

function Get_Usages() {
    if (document.forms[0].showusage.checked) {
        var rU = new ajaxRequest();
        rU.setCallBackFunc(function () { Get_Usages_Ans(rU); });
        var params = "Function=Get_Usage";
        params += "&sDate=" + encodeURI((sDate.getMonth() + 1) + '/' + sDate.getDate() + '/' + sDate.getFullYear());
        params += "&Rooms=" + encodeURI(aRooms.join(','));
        params += "&gridtop=" + parseInt(gridTop);
        params += "&iheight=" + parseInt(iHeight);
        params += "&iwidth=" + parseInt(iWidth);
        params += "&gridLeft=" + parseInt(gridLeft);
        clear_bgs();
        rU.doPost(sModulePath, params);
        setPointer(1);
    }
    //document.getElementById('message').innerHTML = 'lll';
}

function clear_bgs() {
    setPointer(1);
    for (i = 31; i < 452; i += 30) {
        for (x = 0; x < 30; x++) {
            var tmp = document.getElementById(i + x);
            tmp.style.backgroundColor = '#FFFFFF';
        }
    }
    setPointer(0);
}

function Get_Usages_Ans(rU) {
    var bg = rU.responseText.split('-|-');
    for (i = 31; i < 452; i += 30) {
        for (x = 0; x < 30; x++) {
            var tmp = document.getElementById(i + x);
            tmp.style.backgroundColor = bg[i + x - 31];
        }
    }
    setPointer(0);
}

function Get_Date(newDate) {
    if (sDate != newDate) {
        sDate = new Date(newDate);
        document.getElementById('reservations').innerHTML = "";
        Fill_Dates();
        Get_Reservations();
    }
}

function Last_30() {
    var tDate = new Date();
    tDate.setTime(sDate.getTime() - (30 * 24 * 60 * 60 * 1000));
    Get_Date(tDate);
}

function Last_7() {
    var tDate = new Date();
    tDate.setTime(sDate.getTime() - (7 * 24 * 60 * 60 * 1000));
    Get_Date(tDate);
}

function Next_7() {
    var tDate = new Date();
    tDate.setTime(sDate.getTime() + (7 * 24 * 60 * 60 * 1000));
    Get_Date(tDate);
}

function Next_30() {
    var tDate = new Date();
    tDate.setTime(sDate.getTime() + (30 * 24 * 60 * 60 * 1000));
    Get_Date(tDate);
}

function Jump2Date() {
    var f = document.forms[0].jumpdate;
    if (f.value != '') {
        var tDate = new Date(f.value);
        Get_Date(tDate);
    }
}

function Prev_Rooms(val) {
    aLastRoomListID = ((aLastRoomListID * 1) - (val * 1) < 0) ? 0 : (aLastRoomListID * 1) - (val * 1);
    Get_Rooms();
}

function Next_Rooms(val) {
    aLastRoomListID = (aLastRoomListID * 1) + (val * 1);
    Get_Rooms();
}

function Room_Search() {
    bRoomSearch = true;
    Get_Rooms();
}

function Check_Overlap(reservation) {
    //Find and store Y-Axis (row)
    var x = 0;
    for (i = 1; i <= 421; i += 30) {
        x++;
        var tmp = document.getElementById(i);
        if (posY >= parseInt(tmp.style.top)) {
            rowstart = x
        }
    }
    //Find and store X-Axis (Column)
    x = 0;
    for (i = 1; i < 30; i++) {
        x++;
        var tmp = document.getElementById(i);
        if (posX > parseInt(tmp.style.left)) {
            colstart = x;
        }
    }

    var dragTop = 0;

    //If more than 1 reservation is shown on matrix find array position then check for overlaps
    if (resCount > 1) {

        //Get reservation position in array
        var dragArrayPos = 0;
        var aTitle = reservation.title.split(',');
        for (i = 0; i < aResTops.length; i++) {
            if (aResIDs[i] == parseInt(aTitle[0]) && aResTops[i] == tempy) {
                dragTop = aResTops[i];
                dragArrayPos = i;
            }
        }

        //check to see if overlapping another res
        var overlap = false;
        var overlapid = 0;
        var testmessage = '';
        for (i = 0; i < resCount; i++) {
            if (posY < parseInt(document.getElementById('holder').style.top) && i != dragArrayPos && aResTops[i] == parseInt(reservation.style.top) && aResLefts[i] >= parseInt(reservation.style.left) && aResLefts[i] < parseInt(reservation.style.width) + parseInt(reservation.style.left)) {
                if (parseInt(aResWidth[i]) == parseInt(aResWidth[dragArrayPos])) {
                    if (confirm('overlapping reservations\nDo you want to swap rooms?')) {	//overlap
                        document.getElementById('res' + i).style.top = dragTop + 'px'; //y-((y-maxTop)%rowHeight);
                        aResTops[i] = aResTops[dragArrayPos];
                        aResTops[dragArrayPos] = parseInt(reservation.style.top);
                        Update_Reservation(reservation, true, document.getElementById('res' + i), aResTops[i]);
                    } else { //reset reservation
                        reservation.style.top = aResTops[dragArrayPos];
                    }
                } else {
                    alert('Cannot swap rooms if reservations have different lengths of stay.\nResetting rooms');
                    reservation.style.top = dragTop;
                }
                overlap = true;
            }
            testmessage += aResIDs[i] + "'s Top = " + aResTops[i] + ", i=" + i + "\n";
        }
        testmessage += "dragArrayPos=" + dragArrayPos + "\n";
        if (!overlap) {
            Update_Reservation(reservation, overlap, null, aResTops[dragArrayPos]);
            aResTops[dragArrayPos] = parseInt(reservation.style.top);
        }
    } else {
        Update_Reservation(reservation, false, null, aResTops);
        aResTops = parseInt(reservation.style.top);

    }
    //alert(testmessage);
    //alert(aResTops[dragArrayPos] + ", ID=" + aResIDs[dragArrayPos]);
    //alert('X=' + posX + '\nY=' + posY + '\nRow = ' + rowstart + '\nColumn = ' + (colstart+1) + '\nsquare height='+document.getElementById('1').style.height+'\nres height='+reservation.style.height);
}

function Edit_Res() {
    document.getElementById('popedit').style.visibility = 'hidden';
    window.navigate("../marketing/editreservation.aspx?reservationid=" + parseInt(oRes.title));
}

function Drag_Res() {
    initializeDrag(oRes, event);
    document.getElementById('popmenu').style.visibility = 'hidden';
    document.getElementById('popedit').style.visibility = 'hidden';
    document.getElementById('popdrag').style.visibility = 'hidden';
    bPopMenu = false;
}

function Update_Reservation(reservation, overlap, overlapreservation, resOldTop) {
    var reqUPRes = new ajaxRequest();
    reqUPRes.setCallBackFunc(function () { Update_Reservation_Ans(reqUPRes, reservation, overlap, overlapreservation); });
    var params = "Function=Update_Reservation";
    var t = reservation.title.split(',');
    params += "&ID=" + parseInt(t[0]);
    params += "&Rooms=" + aRooms.join(',');
    params += "&Row1=" + ((parseInt(reservation.style.top) - parseInt(gridTop) - parseInt(iHeight) - parseInt(iHeight)) / parseInt(iHeight)) + '';
    params += "&Row2=" + ((parseInt(resOldTop) - parseInt(gridTop) - parseInt(iHeight) - parseInt(iHeight)) / parseInt(iHeight)) + '';
    params += "&RoomTypes=" + aRoomType.join(',');
    params += "&overlap=" + overlap;
    if (overlap) {
        t = overlapreservation.title.split(',');
        params += "&overlapid=" + parseInt(t[0]);
    }
    //alert(params);
    reqUPRes.doPost(sModulePath, params);
    document.forms[0].disabled = true;
    setPointer(1);
}

function Update_Reservation_Ans(reqUPRes, reservation, overlap, overlapreservation) {
    alert(reqUPRes.responseText); // + "\nID=" + parseInt(reservation.title) + "\noverlap=" + overlap );
    if (overlap) {
        //alert(parseInt(overlapreservation.title));
    }
    reqUPRes = null;
    document.forms[0].disabled = false;
    setPointer(0);
    Get_Reservations();
}

function setPointer(value) {
    if (value == 1) {
        if (document.all) {
            for (var i = 0; i < document.all.length; i++) {
                document.all(i).style.cursor = "wait";
            }
        }
    } else if (value == 0) {
        if (document.all) {
            for (var i = 0; i < document.all.length; i++) {
                document.all(i).style.cursor = "default";
            }
        }
    }
}

function Allocations() {
    var bAllocating = (document.forms[0].allocate.checked);
    document.getElementById("allocation").style.display = (bAllocating) ? '' : 'none';
    document.getElementById("holder").style.display = (bAllocating) ? 'none' : '';
    Hide_Show_Holders(-1, true);
    document.getElementById("holderentries").style.display = (bAllocating) ? 'none' : '';
    document.getElementById("key").style.top = (bAllocating) ? '800px' : '750px';
}

function Add_Room(cbo) {
    var t = document.forms[0].roomssel;
    var bFound = false;
    for (i = 0; i < t.options.length; i++) {
        if (t.options[i].value == cbo.options[cbo.selectedIndex].value) { bFound = true; }
    }
    if (cbo.selectedIndex > -1 && !bFound) { t.options[t.options.length] = new Option(cbo.options[cbo.selectedIndex].text, cbo.options[cbo.selectedIndex].value); }
    if (cbo.selectedIndex < cbo.options.length - 1) { cbo.selectedIndex++; }
}

function Remove(cbo) {
    if (cbo.selectedIndex == -1 && cbo.options.length > 0) { cbo.selectedIndex = cbo.options.length - 1; }
    (cbo.selectedIndex > -1) ? cbo.options[cbo.selectedIndex] = null : null;
}

function Do_Allocation() {
    if (Valid_Allocation()) {
        var a = new ajaxRequest();
        var f = document.forms[0];
        var params = 'function=allocate';
        a.setCallBackFunc(function () { Do_Allocation_Ans(a); });
        params += '&sdate=' + f.sdate.value;
        params += '&edate=' + f.edate.value;
        params += '&type=' + f.alltype.options[f.alltype.selectedIndex].value;
        params += '&rooms=';
        for (i = 0; i < f.roomssel.options.length; i++) {
            params += (i == 0) ? '' : ',';
            params += f.roomssel.options[i].value
        }
        a.doPost(sModulePath, params);
        f.disabled = true;
        setPointer(1);
    }
}

function Valid_Allocation() {
    var sErr = '';
    var f = document.forms[0];
    sErr += (f.sdate.value == '') ? 'Please Enter a Starting Date.\n' : '';
    sErr += (f.edate.value == '') ? 'Please Enter an Ending Date.\n' : '';
    sErr += (f.roomssel.options.length < 1) ? 'Please Select a Room to Allocate.\n' : '';
    sErr += (f.alltype.selectedIndex < 1) ? 'Please Select a Usage Type.\n' : '';

    (sErr == '') ? null : alert(sErr);
    return (sErr == '') ? true : false;
}

function Do_Allocation_Ans(a) {
    var f = document.forms[0];
    alert(a.responseText);
    f.disabled = false
    for (i = f.roomssel.options.length; i > -1; i--) {
        f.roomssel.options[i] = null;
    }
    setPointer(0);
    if (f.showusage.checked) { Get_Usages(); }
}
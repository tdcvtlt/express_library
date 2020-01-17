
function jsPop(jsURL, jsWindowNm, jsWidth, jsHeight)
    {
        var hdl;
        if(jsURL!="")
        {
            var jsoption = "scrollbars=yes,resizable=no,width=" + jsWidth + ",height=" + jsHeight;
            hdl = window.open(jsURL,jsWindowNm,jsoption );
        }
        return hdl;
    } 



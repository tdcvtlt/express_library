


// The constructor for the ajaxRequest object.
function ajaxRequest()
{
	// Create the actual xmlHttpRequest object
	this.req = false;
	
	// Branch for native XMLHttpRequest object
	if( window.XMLHttpRequest )
	{
		try
		{
			this.req = new XMLHttpRequest();
		}// End try
		catch( e )
		{
			this.req = false;
		}// End catch
	// branch for IE/Windows ActiveX version
	} 
	else if( window.ActiveXObject )
	{
		try
		{
			this.req = new ActiveXObject( "Msxml2.XMLHTTP" );
		}// End try
		catch( e )
		{
			try
			{
				this.req = new ActiveXObject( "Microsoft.XMLHTTP" );
			}// End try
			catch( e )
			{
				this.req = false;
			}// End catch
		}
	}// End else if
	
	// Create a named instance of this object
	var myRequest = this;
	
	// Sets the function to call when the request returns from the server
	this.setCallBackFunc = function( f )
	{
	   myRequest.callBack = f;
	}// End ajaxRequest::setCallBackFunc
	
	// The function that is called when the request returns.  This function also
	// calls the function that the user set thru setCallBackFunc
	this.processReqChange = function()
	{
		// If req's state is "loaded"
		if( myRequest.req.readyState == 4 )
		{	    
			// If the request's status is "OK"
			if( myRequest.req.status == 200 )
			{
				// Get the response.
				myRequest.responseText = myRequest.req.responseText;
				myRequest.responseXML = myRequest.req.responseXML;
				if( myRequest.callBack )
					myRequest.callBack();
			}// End if
			else
			{
				if( myRequest.req.status == 500 ){
					myRequest.responseText = myRequest.req.responseText;
					myRequest.handleErrFullPage(myRequest.responseText);
				}else{
					alert( "There was a problem retrieving the XML data:\n" +
					   myRequest.req.statusText + "Status:\n" + myRequest.req.status );
				}
			}// End else
		}// End if
	}// End ajaxRequest::processReqChange
	
	this.req.onreadystatechange = this.processReqChange;
	
	// Sets the request headers
	this.setRequestHeader = function( type, data )
	{
		myRequest.req.setRequestHeader( type, data );
	}// End ajaxRequest::setRequestHeader
	
	// Opens the request
	this.open = function( method, address, asynch )
	{
		myRequest.req.open( method, address, asynch );
	}// End ajaxRequest::open

	// Sends this request along with the data
	this.send = function( data )
	{
		myRequest.req.send( data );
	}// End ajaxRequest::send
	
	// Does a post request to url and sends data
	this.doPost = function( url, data )
	{
		var contentType = "application/x-www-form-urlencoded; charset=UTF-8";

		myRequest.req.open( "POST", url, true );
		myRequest.req.setRequestHeader( "Content-Type", contentType );
		myRequest.req.send( data );
	}// End ajaxRequest::doPost
	
	// Shows the response text in a new div tag
	this.debug = function()
	{
		var div = document.createElement( 'div' );
		div.innerHTML = myRequest.req.responseText;
		div.style.backgroundColor = '#FFFFFF';
		div.style.color = '#000000';
		document.body.appendChild( div );
	}// End ajaxRequest.debug
	
	this.handleErrFullPage = function(strIn) {

        var errorWin;

        // Create new window and display error
        try {
                errorWin = window.open('', 'errorWin');
                errorWin.document.body.innerHTML = strIn;
        }
        // If pop-up gets blocked, inform user
        catch(e) {
                alert('An error occurred, but the error message cannot be' +
                        ' displayed because of your browser\'s pop-up blocker.\n' +
                        'Please allow pop-ups from this Web site.');
       }
	}
}// End ajaxRequest::Constructor



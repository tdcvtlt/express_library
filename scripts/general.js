function popitup(url)
	{
	    modal.mwindow.open(url,'Edit',450,250);
		//newwindow=window.open(url,'_blank','height=250,width=450,top=' + ((screen.height/2) - 162) + ',left=' + ((screen.width/2) - 261) + ',scrollbars=yes');
		//window.showModalDialog(url,'_blank','dialogHeight:250px;dialogWidth:450px;center:yes;scrollbars:yes;status:no;');
		//if (window.focus) {newwindow.focus()}
		return false;
	}
	
function popitup2(url)
{
    modal.mwindow.open(url,'Edit',690,450);
	//newwindow=window.open(url,'_blank','height=450,width=690,top=' + ((screen.height/2) - 200) + ',left=' + ((screen.width/2) - 350) + ',scrollbars=yes');
	//window.showModalDialog(url,'_blank','dialogHeight:450px;dialogWidth:750px;center:yes;scrollbars:yes;status:no;');
	//if (window.focus) {newwindow.focus()}
	return false;
}
function popitup3(url)
{
    modal.mwindow.open(url,'Edit',900,450);
	//newwindow=window.open(url,'_blank','height=450,width=900,top=' + ((screen.height/2) - 200) + ',left=' + ((screen.width/2) - 350) + ',scrollbars=yes');
	//window.showModalDialog(url,'_blank','dialogHeight:450px;dialogWidth:750px;center:yes;scrollbars:yes;status:no;');
	//if (window.focus) {newwindow.focus()}
	return false;
}
function Format_Currency(value,comma){
	var dot=false;
	var temp = value + '';
	//temp = Math.round(temp,2);
	(comma=='True')?comma=true:null;
	(comma==false)?null:comma=true;
	(temp=='')?temp='0':null;
	for(l=0;l<temp.length;l++){
		if (temp.charAt(l) == '.') {
			(l==temp.length-3)?null:(l==temp.length-2)?temp+= '0':(l<temp.length -4)?temp=(Math.round(temp*100))/100:(l==0)?temp='0' + temp:temp+='00';
			dot = true;
			break;
		}
	}
	(dot==true)?null:temp+='.00';
	if (temp * 1 > 1000){
		var hold = temp;
		var pos = -1;
		temp = '';
		for(l=hold.length -1;l>=0;l--){
			if ((hold.charAt(l) == '.' || pos > -1) && (comma==true)){
				pos++;
				(pos%3==0 && pos>0)?temp=''+hold.charAt(l)+temp:temp=hold.charAt(l)+temp;
			} else {
				temp=hold.charAt(l)+temp;
			}
		}
	}
	return temp;
}

function formatCurrency(num) {
	num = num.toString().replace(/\$|\,/g,'');
	if(isNaN(num))
	num = "0";
	sign = (num == (num = Math.abs(num)));
	num = Math.floor(num*100+0.50000000001);
	cents = num%100;
	num = Math.floor(num/100).toString();
	if(cents<10)
	cents = "0" + cents;
	for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
	num = num.substring(0,num.length-(4*i+3))+','+
	num.substring(num.length-(4*i+3));
//	return (((sign)?'':'-') + '$' + num + '.' + cents);
	return (((sign)?'':'-') + '' + num + '.' + cents);
}

function popitmodal(url){
	window.showModalDialog(url,window,'dialogHeight:450px;dialogWidth:690px;resizable:no;scroll:yes;status:no;');
}

function popitmodal2(url){
	window.showModalDialog(url,window,'dialogHeight:250px;dialogWidth:450px;resizable:no;scroll:yes;status:no;');
}
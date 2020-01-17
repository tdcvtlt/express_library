var modal={};

modal.mwindow=function(){
    var mWin;
    var m;
   
    return{
        open:function(path,name,w,h){
            m=document.createElement('div'); 
			m.id='tinymask';
			document.body.appendChild(m); 
			m.style.height=modal.page.total(1)+'px';
			m.style.width=''; m.style.width=modal.page.total(0)+'px';
			this.mask();
			this.alpha(m,1,80,3);
            w==0?w=300:w=w;
            h==0?h=300:h=h;
			var t=(modal.page.height()/2)-(0); t=t<10?10:t;
			var top =(t+modal.page.top())+'px';
			var left = (modal.page.width()/2)-(w/2)+'px'
			mWin = window.open(path,'name','scrollbars=yes,resizable=no,width=' + w + ',height=' + h + ',top=' + top + ',left=' + left);
		
            m.onclick = function(){modal.mwindow.close_it(0)};
            setTimeout(modal.mwindow.pop_check,1000);            
        },
        pop_check:function(){
            if (!mWin.closed)
            {
                setTimeout(modal.mwindow.pop_check,1000);
            }
            else
            {
                modal.mwindow.close_it(1);
            }
        },
        close_it:function(s){
            //alert('hi');
            if (!s)
            {
                mWin.close();
            }
            m.style.opacity=0; m.style.filter='alpha(opacity=0)';
            m.style.display='none';
            this.alpha(m,-1,0,2);
            //m.onclick=null;
        },
        mask:function(){
			m.style.height=modal.page.total(1)+'px';
			m.style.width=''; m.style.width=modal.page.total(0)+'px'
		},
		alpha:function(e,d,a){

			clearInterval(e.ai);

			if(d==1){

				e.style.opacity=0; e.style.filter='alpha(opacity=0)';

				e.style.display='block'; this.pos()

			}

			e.ai=setInterval(function(){modal.mwindow.ta(e,a,d)},20)

		},

		ta:function(e,a,d){

			var o=Math.round(e.style.opacity*100);

			if(o==a){

				clearInterval(e.ai);

				if(d==-1){

					e.style.display='none';

					//if(e==p)modal.window.alpha(m,-1,0,2);//:b.innerHTML=p.style.backgroundImage=''

				}

			}else{

				var n=Math.ceil((o+((a-o)*.5))); n=n==1?0:n;

				e.style.opacity=n/100; e.style.filter='alpha(opacity='+n+')'

			}

		},

		pos:function(){

			

		}
    }
}();

modal.page=function(){

	return{

		top:function(){return document.documentElement.scrollTop||document.body.scrollTop},

		width:function(){return self.innerWidth||document.documentElement.clientWidth||document.body.clientWidth},

		height:function(){return self.innerHeight||document.documentElement.clientHeight||document.body.clientHeight},

		total:function(d){

			var b=document.body, e=document.documentElement;

			return d?Math.max(Math.max(b.scrollHeight,e.scrollHeight),Math.max(b.clientHeight,e.clientHeight)):

			Math.max(Math.max(b.scrollWidth,e.scrollWidth),Math.max(b.clientWidth,e.clientWidth))

		}

	}

}();

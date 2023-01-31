import { Component,OnInit } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

 constructor(private authService:AuthService){}
status: string | undefined;
 ngOnInit():void {
  this.authService.userManager.getUser().then((user)=>{
    if(user){
      this.status="Hoşgeldiniz";
      console.log(user);
    }
    else{
      this.status="Giriş yapılmadı";
    }
   });
}
login():void{
  this.authService.userManager.signinRedirect();
}
logout():void{
  this.authService.userManager.signoutRedirect();
}
 
}

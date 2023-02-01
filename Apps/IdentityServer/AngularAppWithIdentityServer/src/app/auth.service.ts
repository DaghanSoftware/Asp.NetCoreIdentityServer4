import { Injectable } from '@angular/core';
import * as oidc from "oidc-client";
@Injectable({
  providedIn: 'root'
})
export class AuthService {
config ={
  //Bu uygulamanın yetkiyi alacağı sunucunun yani Auth Server’ın adresini tutar.
  authority:'https://localhost:7139',
  //Client’ın Auth Server’da var olan tanımlamasındaki client id değerini tutar.
  client_id:'js-client',
  //Client doğrulandığı zaman hangi adrese yönlendirme yapılacağını tutar. Burada dikkat ederseniz ‘/callback’ adresine yönlendirme gerçekleştirilmektedir. Birazdan bu adresi karşılayacak olan component oluşturulacaktır.
  redirect_uri:'http://localhost:4200/callback',
  //Akış türünü belirtir. ‘code’, authorization code’a karşılık gelir.
  response_type:'code',
  //Client’ın talep ettiği scope’lardır. Bir MVC uygulamasına nazaran scope’lar oldukça kolay talep edilebilmektedir. Scope’lar arasında virgül vs. gibi bir ayraç olmaksızın direkt boşluk kullanılmalıdır.
  scope:"openid profile email api1.read",
  //Çıkış yapıldığında kullanıcının hangi adrese yönlendirileceği tutulur.
  post_logout_redirect_uri:'http://localhost:4200',
};
userManager;
  constructor() {
    this.userManager=new oidc.UserManager(this.config);
   }
}

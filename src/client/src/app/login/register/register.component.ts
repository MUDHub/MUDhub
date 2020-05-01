import { Component } from "@angular/core";
import { FormControl } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { IUser } from "src/app/model/IUser";

@Component({
	selector: "mh-register",
	templateUrl: "./register.component.html",
	styleUrls: ["./register.component.scss"],
})
export class RegisterComponent {
	constructor(private authService: AuthService) {}

	firstname = new FormControl();
	lastname = new FormControl();
	mail = new FormControl();
	password = new FormControl();

	register() {
		let user: IUser = {
			name: this.firstname.value,
			lastname: this.lastname.value,
			email: this.mail.value,
			password: this.password.value,
		};
		this.authService.register(user);
	}
}

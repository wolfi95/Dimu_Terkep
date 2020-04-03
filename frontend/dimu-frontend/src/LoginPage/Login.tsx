import React from "react";
import Axios, { AxiosResponse } from "axios";

interface ILoginState {
  email: string;
  password: string;
}

interface ILoginProps {
    loginSuccess(token:string):void;
}

export class LoginForm extends React.Component<ILoginProps, ILoginState> {
  constructor(props:ILoginProps) {
    super(props);
    this.state = {
      email: "",
      password: "",
    };
  }

  render() {
    return (
      <form onSubmit={this.handleSubmit}>
        <label htmlFor="email">Email</label>
        <input
          name="email"
          type="text"
          placeholder="Enter your email"
          value={this.state.email}
          onChange={this.handleChange}
        />
        <label htmlFor="email">Password</label>
        <input
          name="password"
          type="password"
          placeholder="Enter your password"
          value={this.state.password}
          onChange={this.handleChange}
        />
        <button type="submit">Login</button>
      </form>
    );
  }

  handleChange = (event: React.FormEvent<HTMLInputElement>) => {
    switch (event.currentTarget.name) {
      case "email": {
        this.setState({ email: event.currentTarget.value });
        break;
      }
      case "password": {
        this.setState({ password: event.currentTarget.value });
        break;
      }
    }
  };

  handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    Axios.post("https://localhost:44376/admin/login",{userName:this.state.email, password:this.state.password})
        .then(result => {
            if(result.status == 200){
                console.log(result.data);
                this.props.loginSuccess(result.data);
            }else if(result.status == 401){
                console.log("asdasdasd" + result.data);
            }
        })
        .catch((error:any) => {
            console.log("error: " + error.response.data);
        })
    
  };
}

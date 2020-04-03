import React from "react";
import logo from "./logo.svg";
import "./App.css";
import axios from "axios";
import { createBrowserHistory } from "history";
import { Router, Switch, Route, Redirect } from "react-router";
import { LoginForm } from "./LoginPage/Login";
import Button from '@material-ui/core/Button';

interface IState {
  helo: string;
  isLoggedin: boolean;  
}
const history = createBrowserHistory();

class App extends React.Component<{}, IState> {
  constructor(prop: {}) {
    super(prop);
    axios.defaults.baseURL ="https://localhost:44376"    
    let token: string | null;
        if((token = localStorage.getItem("token")) !== null){ 
          axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;           
            this.state = {
              isLoggedin: localStorage.getItem("loginState") === "true" ? true : false
              ,helo:""
            };
        }else {
            this.state = {
              isLoggedin: false
              ,helo:""
            };
        }
  }

  loginSuccess = (token:string) => {

    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    localStorage.setItem("loginState","true");
    localStorage.setItem("token",token);

    this.setState({isLoggedin:true}, () => {
      axios
      .get<string>("/admin/hello")
      .then((val) => {
        console.log(val.data);
        this.setState({ helo: val.data },() => {
          history.push("/helo")
        });
      })
      .catch((err) => {
        //productionben ilyen nem maradhat
        console.log(err);
      });
      
    })
  }

  onLogoutClick = () => {
    localStorage.setItem("loginState","false");
    localStorage.setItem("token","");
  }

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <Router history={history}>
            <Switch>
              <Route path="/login">
                <LoginForm loginSuccess={this.loginSuccess}/>
              </Route>              
              <Route path="/helo">
                {this.state.isLoggedin ? (
                  <p>{this.state.helo}</p>
                ) : (
                  <Redirect to="/login"></Redirect>
                )}
              </Route>
            </Switch>
          </Router>
          <a
            className="App-link"
            href="https://reactjs.org"
            target="_blank"
            rel="noopener noreferrer"
          >
            Learn React
          </a>
        </header>
        <Button onClick={this.onLogoutClick}>
          Log out
        </Button>
      </div>
    );
  }
}

export default App;

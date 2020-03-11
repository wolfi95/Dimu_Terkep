import React from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios'

interface IState {
  helo:string;
}

class App extends React.Component<{},IState> {

  constructor(prop:{}) {
    super(prop);
    this.state = { helo: ""}
  }
    

  componentDidMount() {
    axios.get<string>("https://localhost:44376/admin/hello")
    .then(val => {
      console.log(val.data);      
      this.setState({helo: val.data})
      })
    .catch(err =>{
      //productionben ilyen nem maradhat      
      console.log(err)
    });      
  }

  render(){
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <p>
            { this.state.helo }
          </p>
          <a
            className="App-link"
            href="https://reactjs.org"
            target="_blank"
            rel="noopener noreferrer"
          >
            Learn React
          </a>
        </header>
      </div>
    );
  }
}

export default App;

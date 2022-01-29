import logo from './logo.svg';
import './App.css';
import Speech from 'react-speech';
import React, { useState, useEffect } from 'react';
//'Hello Dan. I am alive! Alive I tell you! Alive! Fear me!'

//https://www.npmjs.com/package/react-speech


function App() {
  useEffect(() => {
    // code to run after render goes here
    document.getElementsByClassName("rs-play")[0].innerHTML = "Play";
    document.getElementsByClassName("rs-pause")[0].innerHTML = "Pause";
    document.getElementsByClassName("rs-resume")[0].innerHTML = "Resume";
    document.getElementsByClassName("rs-stop")[0].innerHTML = "Stop";
  });

  return (
    <div className="App">
      <h1>Text-To-Speech</h1>
      <p>"Hello human. I am alive! Alive I tell you! Alive! Fear me!"</p>
      <Speech text="Hello human. I am alive! Alive I tell you! Alive! Fear me!" voice="Google UK English Female" pause={true} resume={true}  lang="en-US" stop={true} />
    </div>
  );
}

export default App;

import React from "react";
import {
    BrowserRouter as Router,
    Switch,
    Route
} from "react-router-dom";

import Header from "components/Header";

import Home from "components/pages/Home";
import Options from "components/pages/Options";
import Login from "components/pages/Login";
import Signup from "components/pages/Signup";
import StudentPortal from "./pages/StudentPortal";

import styles from "./App.module.scss";

const App: React.FC = () => {
    return (
        <div className={styles.app}>
            <Router>
                <Header />
                <Switch>
                    <Route path="/options">
                        <Options />
                    </Route>
                    <Route path="/login">
                        <Login />
                    </Route>
                    <Route path="/signup">
                        <Signup />
                    </Route>
                    <Route path="/student">
                        <StudentPortal />
                    </Route>
                    <Route path="/">
                        <Home />
                    </Route>
                </Switch>
            </Router>
        </div>
    );
};

export default App;
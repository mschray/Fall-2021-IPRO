import React from "react";

const Login: React.FC = () => {
    const handleSubmit = (event: { preventDefault: () => void; }) => {
        alert('Login Successfully.')
      }
    return (
         <div className="wrapper">
             <h1>Login</h1>
             <form onSubmit={handleSubmit}>
                 <label>
                     <p>Email</p>
                     <input type="email" name="email" />
                     <p>Password</p>
                     <input type="password" name="password" />
                 </label>
                 <p></p>
                 <button type="submit">Login</button>
             </form>
         </div>
    );
};

export default Login;
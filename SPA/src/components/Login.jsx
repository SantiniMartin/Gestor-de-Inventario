import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        setError('');
        if (!login(email, password)) {
            setError('Correo o contraseña incorrectos.');
        } else {
            navigate('/');
        }
    };

    return (
        <div className="auth-container">
            <form onSubmit={handleSubmit} className="auth-form">
                <h2>Iniciar Sesión</h2>
                {error && <p className="auth-error">{error}</p>}
                <div className="form-group">
                    <label htmlFor="email">Correo Electrónico</label>
                    <input
                        type="email"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Contraseña</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit" className="auth-button">Entrar</button>
                <p className="auth-switch">
                    ¿No tienes una cuenta? <Link to="/register">Regístrate</Link>
                </p>
            </form>
        </div>
    );
};

export default Login;

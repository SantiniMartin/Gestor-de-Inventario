import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { register } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        setError('');
        const { success, message } = register(email, password);
        if (!success) {
            setError(message);
        } else {
            navigate('/');
        }
    };

    return (
        <div className="auth-container">
            <form onSubmit={handleSubmit} className="auth-form">
                <h2>Crear Cuenta</h2>
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
                <button type="submit" className="auth-button">Registrarse</button>
                <p className="auth-switch">
                    ¿Ya tienes una cuenta? <Link to="/login">Inicia Sesión</Link>
                </p>
            </form>
        </div>
    );
};

export default Register;

import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

const Login = () => {
    const [nombreUsuario, setNombreUsuario] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        const success = await login(nombreUsuario, password);
        if (!success) {
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
                    <label htmlFor="nombreUsuario">Nombre de Usuario (Email)</label>
                    <input
                        type="email"
                        id="nombreUsuario"
                        value={nombreUsuario}
                        onChange={(e) => setNombreUsuario(e.target.value)}
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
                <div className="auth-forgot-password" style={{ marginTop: '15px', textAlign: 'center' }}>
                    <Link to="/forgot-password">¿Olvidaste tu contraseña?</Link>
                </div>
                <p className="auth-switch">
                    ¿No tienes una cuenta? <Link to="/register">Regístrate</Link>
                </p>
            </form>
        </div>
    );
};

export default Login;

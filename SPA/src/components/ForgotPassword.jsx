import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { API_BASE_URL } from '../config';

const ForgotPassword = () => {
    const navigate = useNavigate();
    const [nombreUsuario, setNombreUsuario] = useState('');
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage('');
        setError('');

        try {
            const response = await fetch(`${API_BASE_URL}/forgot-password`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ nombreUsuario }),
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || 'Error al solicitar el reseteo de contraseña.');
            }
            console.log("aca esta el token",data)
            if (data.token) {
                // Construir la ruta de reseteo con el token del backend
                const resetPath = `/reset-password?token=${data.token}`;
                console.log(data.token)
                // Redirigir usando React Router
                setMessage('Redirigiendo a la página de reseteo...');
                navigate(resetPath);
            } else {
                // Fallback por si la URL no viene
                setMessage('Si existe una cuenta con ese nombre de usuario, se han enviado instrucciones para restablecer la contraseña.');
            }

        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="auth-container">
            <form onSubmit={handleSubmit} className="auth-form">
                <h2>Recuperar Contraseña</h2>
                <p>Ingresa tu nombre de usuario y te enviaremos (simularemos) un enlace para restablecer tu contraseña.</p>
                <div className="form-group">
                    <label htmlFor="username">Nombre de Usuario</label>
                    <input
                        type="text"
                        id="username"
                        value={nombreUsuario}
                        onChange={(e) => setNombreUsuario(e.target.value)}
                        required
                    />
                </div>
                <button type="submit" className="auth-button">Enviar</button>
                {message && <p className="success-message">{message}</p>}
                {error && <p className="error-message">{error}</p>}
            </form>
        </div>
    );
};

export default ForgotPassword;

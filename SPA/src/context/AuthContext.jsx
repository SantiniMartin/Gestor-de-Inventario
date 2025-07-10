import React, { useState, useEffect } from 'react';
import { API_BASE_URL } from '../config';

import { AuthContext } from './AuthContextObject';

export const AuthProvider = ({ children }) => {
    const [currentUser, setCurrentUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Comprobar si hay un usuario logueado en localStorage al cargar la app
        const loggedInUser = localStorage.getItem('currentUser');
        if (loggedInUser) {
            setCurrentUser(JSON.parse(loggedInUser));
        }
        setLoading(false);
    }, []);

    const login = async (nombreUsuario, password) => {
        try {
            const response = await fetch(`${API_BASE_URL}/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ nombreUsuario, password })
            });

            if (!response.ok) {
                return false; // Falla el login
            }

            const userData = { nombreUsuario }; // Guardamos el nombre de usuario
            setCurrentUser(userData);
            localStorage.setItem('currentUser', JSON.stringify(userData));
            return true;
        } catch (error) {
            console.error("Error en el login:", error);
            return false;
        }
    };

    const register = async (nombreUsuario, password) => {
        try {
            const response = await fetch(`${API_BASE_URL}/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ nombreUsuario, password })
            });

            if (!response.ok) {
                const errorData = await response.json();
                return { success: false, message: errorData.message || 'Error al registrar.' };
            }

            // Opcional: auto-login después del registro
            await login(nombreUsuario, password);

            return { success: true };
        } catch (error) {
            console.error("Error en el registro:", error);
            return { success: false, message: 'No se pudo conectar con el servidor.' };
        }
    };

    const logout = () => {
        setCurrentUser(null);
        localStorage.removeItem('currentUser');
    };

    const value = {
        currentUser,
        login,
        register,
        logout,
    };

    return (
        <AuthContext.Provider value={value}>
            {!loading && children}
        </AuthContext.Provider>
    );
};



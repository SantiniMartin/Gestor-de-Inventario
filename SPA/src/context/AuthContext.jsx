import React, { createContext, useState, useEffect, useContext } from 'react';
import bcrypt from 'bcryptjs';

const AuthContext = createContext();

const salt = bcrypt.genSaltSync(10);
const defaultUserEmail = 'usuario@gmail.com';
const defaultUserPass = 'admin';
const hashedDefaultPass = bcrypt.hashSync(defaultUserPass, salt);

export const AuthProvider = ({ children }) => {
    const [currentUser, setCurrentUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Pre-load the default user if not present
        const users = JSON.parse(localStorage.getItem('users')) || {};
        if (!users[defaultUserEmail]) {
            users[defaultUserEmail] = { password: hashedDefaultPass };
            localStorage.setItem('users', JSON.stringify(users));
        }

        // Check for a logged-in user in localStorage on initial load
        const loggedInUser = JSON.parse(localStorage.getItem('loggedInUser'));
        if (loggedInUser) {
            setCurrentUser(loggedInUser);
        }
        setLoading(false);
    }, []);

    const login = (email, password) => {
        const users = JSON.parse(localStorage.getItem('users')) || {};
        const user = users[email];

        if (user && bcrypt.compareSync(password, user.password)) {
            const userData = { email };
            setCurrentUser(userData);
            localStorage.setItem('loggedInUser', JSON.stringify(userData));
            return true;
        }
        return false;
    };

    const register = (email, password) => {
        const users = JSON.parse(localStorage.getItem('users')) || {};
        if (users[email]) {
            return { success: false, message: 'El correo electrónico ya está registrado.' };
        }
        const hashedPassword = bcrypt.hashSync(password, salt);
        users[email] = { password: hashedPassword };
        localStorage.setItem('users', JSON.stringify(users));
        
        // Automatically log in the user after registration
        const userData = { email };
        setCurrentUser(userData);
        localStorage.setItem('loggedInUser', JSON.stringify(userData));
        
        return { success: true };
    };

    const logout = () => {
        setCurrentUser(null);
        localStorage.removeItem('loggedInUser');
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

export const useAuth = () => {
    return useContext(AuthContext);
};

import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { API_BASE_URL } from '../config';

const CreateProduct = () => {
    const [producto, setProducto] = useState({
        nombre: '',
        descripcion: '',
        precio: 0,
        stock: 0,
        marca: '',
        imagenUrl: '',
        categoriaId: ''
    });
    const [categorias, setCategorias] = useState([]);
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        // Cargar las categorías para el dropdown
        const fetchCategorias = async () => {
            try {
                const response = await fetch(`${API_BASE_URL}/categorias`);
                if (!response.ok) {
                    throw new Error('No se pudieron cargar las categorías.');
                }
                const data = await response.json();
                setCategorias(data);
                if (data.length > 0) {
                    // Preseleccionar la primera categoría
                    setProducto(p => ({ ...p, categoriaId: data[0].id }));
                }
            } catch (err) {
                setError(err.message);
            }
        };

        fetchCategorias();
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setProducto(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage('');
        setError('');

        // Convertir precio y stock a números
        const productoParaEnviar = {
            ...producto,
            precio: parseFloat(producto.precio),
            stock: parseInt(producto.stock, 10),
            categoriaId: parseInt(producto.categoriaId, 10)
        };

        try {
            const response = await fetch(`${API_BASE_URL}/postproductos`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(productoParaEnviar),
            });

            if (!response.ok) {
                const errorData = await response.text();
                throw new Error(errorData || 'Error al crear el producto.');
            }

            setMessage('¡Producto creado con éxito! Redirigiendo a la lista de productos...');
            setTimeout(() => {
                navigate('/');
            }, 2000);

        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="auth-container">
            <form onSubmit={handleSubmit} className="auth-form">
                <h2>Crear Nuevo Producto</h2>

                <div className="form-group">
                    <label htmlFor="nombre">Nombre</label>
                    <input type="text" name="nombre" value={producto.nombre} onChange={handleChange} required />
                </div>

                <div className="form-group">
                    <label htmlFor="descripcion">Descripción</label>
                    <textarea name="descripcion" value={producto.descripcion} onChange={handleChange} required />
                </div>

                <div className="form-group">
                    <label htmlFor="precio">Precio</label>
                    <input type="number" name="precio" value={producto.precio} onChange={handleChange} required step="0.01" />
                </div>

                <div className="form-group">
                    <label htmlFor="stock">Stock</label>
                    <input type="number" name="stock" value={producto.stock} onChange={handleChange} required />
                </div>

                <div className="form-group">
                    <label htmlFor="marca">Marca</label>
                    <input type="text" name="marca" value={producto.marca} onChange={handleChange} />
                </div>

                <div className="form-group">
                    <label htmlFor="imagenUrl">URL de la Imagen</label>
                    <input type="text" name="imagenUrl" value={producto.imagenUrl} onChange={handleChange} />
                </div>

                <div className="form-group">
                    <label htmlFor="categoriaId">Categoría</label>
                    <select name="categoriaId" value={producto.categoriaId} onChange={handleChange} required>
                        <option value="" disabled>Selecciona una categoría</option>
                        {categorias.map(cat => (
                            <option key={cat.id} value={cat.id}>
                                {cat.nombre}
                            </option>
                        ))}
                    </select>
                </div>

                <button type="submit" className="auth-button">Crear Producto</button>
                {message && <p className="success-message">{message}</p>}
                {error && <p className="error-message">{error}</p>}
            </form>
        </div>
    );
};

export default CreateProduct;

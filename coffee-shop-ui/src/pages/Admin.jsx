import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Header } from '../components/Header';
import { richProductsWithImg } from '../js/products';


const Admin = () => {
  const [entityType, setEntityType] = useState('product');
  const [searchQuery, setSearchQuery] = useState('');
  const [operation, setOperation] = useState('create');
  const [formData, setFormData] = useState({});
  const [data, setData] = useState([]);
  const [categoriesList, setCategoriesList] = useState([]);

  const API_BASE_URL = 'http://localhost:5254/api';


  const fetchCategoriesForSelect = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/category`);
      setCategoriesList(response.data);
    } catch (error) {
      console.error('Помилка завантаження списку категорій:', error);
    }
  };
  
 const fetchData = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/${entityType}`);
      let fetchedData = response.data;

      if (entityType === 'product') {
        fetchedData = richProductsWithImg(fetchedData);
      }

      if (searchQuery.trim() !== '') {
        const query = searchQuery.toLowerCase();
        fetchedData = fetchedData.filter(item => {
          if (entityType === 'product') return item.name?.toLowerCase().includes(query);
          if (entityType === 'customer') return item.lastName?.toLowerCase().includes(query) || item.email?.toLowerCase().includes(query);
          if (entityType === 'category') return item.name?.toLowerCase().includes(query);
          return false;
        });
      }

      setData(fetchedData);
    } catch (error) {
      console.error(`Помилка завантаження ${entityType}:`, error);
      setData([]);
    }
  };


  useEffect(() => {
    fetchData();
  }, [entityType, searchQuery]);

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    const targetUrl = `${API_BASE_URL}/${entityType}`;

    let payload = { ...formData };
    
    if (entityType === 'product') {
      payload.price = parseFloat(payload.price || 0);
      payload.stockQuantity = parseInt(payload.stockQuantity || 0);
      payload.roastLevel = parseInt(payload.roastLevel || 0);
      payload.weight = parseFloat(payload.weight || 0);
      payload.categoryId = parseInt(payload.categoryId || 1);
    } else if (entityType === 'customer') {
      payload.subscriptionId = payload.subscriptionId ? parseInt(payload.subscriptionId) : null;
    } else if (entityType === 'subscription') {
      payload.type = payload.type || 'none';
      payload.price = parseFloat(payload.price || 0);
      payload.durationInDays = parseInt(payload.durationInDays || 30);
    }
    else if (entityType === 'order') {
      if (payload.status) payload.status = parseInt(payload.status);

      try {
      
        if (operation === 'create') {
          if (entityType === 'order') return alert('Створення замовлень наразі не підтримується.');
          if (entityType === 'category') return alert('Поки не підтримує створення категорій');
          await axios.post(targetUrl, payload);
          alert(`${entityType} успішно створено!`);
      } else if (operation === 'update') {
          if (!formData.id) return alert('Оберіть ID для оновлення!');
          if (entityType === 'category') return alert('Поки не підтримує оновлення категорій');
          await axios.put(`${targetUrl}/${formData.id}`, payload);
          alert(`${entityType} успішно оновлено!`);
      } else if (operation === 'delete') {
          if (!formData.id) return alert('Оберіть ID для видалення!');
          if (entityType === 'category') return alert('Поки не підтримує створення категорій');
          await axios.delete(`${targetUrl}/${formData.id}`);
          alert(`${entityType} успішно видалено!`);
      }

      setFormData({});
      fetchData(); 

      const idSelect = document.querySelector('select[onChange*="setFormData"]');
      if(idSelect) idSelect.value = "";

    } catch (error) {
      console.error(`Помилка виконання ${operation}:`, error);
      alert(`Помилка: ${error.response?.data || error.message}`);
    }
  };

  

 const renderTableHeaders = () => {
    if (entityType === 'product') {
      return (
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Price</th>
          <th>Stock</th>
          <th>Roast</th>
        </tr>
      );
    } else if (entityType === 'customer') {
      return (
        <tr>
          <th>ID</th>
          <th>First name</th>
          <th>Last name</th>
          <th>Email</th>
          <th>Sub ID</th>
        </tr>
      );
    } else {
      return (
        <tr>
          <th>ID</th>
          <th>Tariff type</th>
          <th>Price</th>
          <th>Duration (days)</th>
        </tr>
      );
    }
  };

  // Функція для генерації рядків таблиці
  const renderTableRows = () => {
  if (data.length === 0) {
    return (
      <tr>
        <td colSpan="5" className="admin-empty">No records found</td>
      </tr>
    );
  }

  return data.map(item => (
    <tr key={item.id}>
      <td>{item.id}</td>

      {entityType === 'product' && (
        <>
          <td><strong>{item.name}</strong></td>
            <td>{item.price} uah</td>
            <td>{item.stockQuantity} шт.</td>
            <td>{item.roastLevel}/5</td>
        </>
      )}

      {entityType === 'user' && (
        <>
            <td>{item.firstName}</td>
            <td><strong>{item.lastName}</strong></td>
            <td>{item.email}</td>
            <td>{item.subscriptionId || '-'}</td>
        </>
      )}

      {entityType === 'subscription' && (
        <>
          <td>
              <strong>
                {item.type}
              </strong>
            </td>
            <td>{item.price} uah</td>
            <td>{item.durationInDays}</td>
        </>
      )}
    </tr>
  ));
  };
  
  const renderFormFields = () => {
    if (operation === 'delete') {
      return <p>Are you sure you want to delete this entry?</p>;
    }

    return (
      <div className="admin-form-inputs">
        {entityType === 'product' && (
          <>
            <input name="name" placeholder="Name" onChange={handleInputChange} className="admin-input" value={formData.name || ''} />
            <input name="description" placeholder="Description" onChange={handleInputChange} className="admin-input" value={formData.description || ''} />
            <input name="price" type="number" step="0.01" placeholder="Price" onChange={handleInputChange} className="admin-input" value={formData.price || ''} />
            <input name="stockQuantity" type="number" placeholder="Stock quantity" onChange={handleInputChange} className="admin-input" value={formData.stockQuantity || ''} />
            <input name="roastLevel" type="number" min="1" max="5" placeholder="Roast level (1-5)" onChange={handleInputChange} className="admin-input" value={formData.roastLevel || ''} />
            <input name="weight" type="number" step="0.1" placeholder="Weight (g)" onChange={handleInputChange} className="admin-input" value={formData.weight || ''} />
            {operation === 'create' && <input name="categoryId" type="number" placeholder="Category ID" onChange={handleInputChange} className="admin-input" value={formData.categoryId || ''} />}
          </>
        )}
        {entityType === 'customer' && (
          <>
            <input name="firstName" placeholder="First name" onChange={handleInputChange} className="admin-input" value={formData.firstName || ''} />
            <input name="lastName" placeholder="Last name" onChange={handleInputChange} className="admin-input" value={formData.lastName || ''} />
            <input name="email" type="email" placeholder="Email" onChange={handleInputChange} className="admin-input" value={formData.email || ''} />
            <input name="subscriptionId" type="number" placeholder="Subscription ID (optional)" onChange={handleInputChange} className="admin-input" value={formData.subscriptionId || ''} />
          </>
        )}
        {entityType === 'subscription' && (
          <>
            <select name="type" onChange={handleInputChange} className="admin-select" value={formData.type || ''}>
              <option value="">Select tariff type</option>
              <option value="1">Basic coffee pass (1)</option>
              <option value="2">Premium roaster club (2)</option>
            </select>
            <input name="price" type="number" step="0.01" placeholder="Price" onChange={handleInputChange} className="admin-input" value={formData.price || ''} />
            <input name="durationInDays" type="number" placeholder="Duration (days)" onChange={handleInputChange} className="admin-input" value={formData.durationInDays || ''} />
          </>
        )}
      </div>
    );
  };

  return (
    <div style={{minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <Header />
      
      <main style={{ paddingTop: '120px', paddingBottom: '60px', flexGrow: 1, backgroundColor: 'var(--color-scheme-1-foreground)'}}>
        <div className="admin-container">
          <h1 className="quality-title">Admin dashboard</h1>
          
          <div className="admin-header-controls">
            <select className="admin-select" value={entityType} onChange={(e) => { setEntityType(e.target.value); setFormData({}); }}>
              <option value="product">Products</option>
              <option value="customer">Customers</option>
              <option value="subscription">Subscriptions</option>
            </select>

            <input type="text" className="admin-input" placeholder={`Search by name...`} value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)} />
          </div>

          <div className="admin-table-wrapper">
            <table className="admin-table">
              <thead>{renderTableHeaders()}</thead>
              <tbody>{renderTableRows()}</tbody>
            </table>
          </div>

          <section className="admin-management-section">
            <h2 className="quality-title-list">Manage data</h2>
            <div className="admin-form-controls">
              <select className="admin-select" value={operation} onChange={(e) => { setOperation(e.target.value); setFormData({}); }}>
                <option value="create">Create</option>
                <option value="update">Update</option>
                <option value="delete">Delete</option>
              </select>
              
              {(operation === 'update' || operation === 'delete') && (
                <select className="admin-select" onChange={(e) => setFormData({id: e.target.value})}>
                  <option value="">Select ID to {operation}</option>
                  {data.map(item => <option key={item.id} value={item.id}>{item.name || item.lastName || `Tariff ${item.type}`} (ID: {item.id})</option>)}
                </select>
              )}
            </div>

            <form onSubmit={handleFormSubmit}>
              {renderFormFields()}
              <button type="submit" className={`admin-btn btn-${operation}`} style={{ marginTop: '20px' }}>
                {operation.toUpperCase()}
              </button>
            </form>
          </section>
        </div>
      </main>

    </div>
  );
};

export default Admin;
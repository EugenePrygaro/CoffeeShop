import React, { useState, useEffect } from 'react';
import { Header } from '../components/Header';
import { richProductsWithImg } from '../js/products';

const Admin = () => {
  const [entityType, setEntityType] = useState('product');
  const [searchQuery, setSearchQuery] = useState('');
  const [data, setData] = useState([]);

    const mockProducts = richProductsWithImg();

  const mockUsers = [
    { id: 1, name: 'Ivan Franko', email: 'ivan@example.com', role: 'admin' },
    { id: 2, name: 'Taras Shevchenko', email: 'taras@example.com', role: 'customer' },
    { id: 3, name: 'Lesya Ukrainka', email: 'lesya@example.com', role: 'customer' },
  ];

  // Імітація запиту до бекенду при зміні типу сутності або пошуку
  useEffect(() => {
    // Тут у майбутньому буде: axios.get(`/api/${entityType}?search=${searchQuery}`)
    
    let currentData = entityType === 'product' ? mockProducts : mockUsers;


    if (searchQuery.trim() !== '') {
      const query = searchQuery.toLowerCase();
      currentData = currentData.filter(item => {

        return (
          item.name.toLowerCase().includes(query) ||
          (item.email && item.email.toLowerCase().includes(query))
        );
      });
    }

    setData(currentData);
  }, [entityType, searchQuery]);

  const renderTableHeaders = () => {
    if (entityType === 'product') {
      return (
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Price (UAH)</th>
          <th>Description</th>
        </tr>
      );
    } else {
      return (
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Email</th>
          <th>Role</th>
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
        <td><strong>{item.name}</strong></td>
        {entityType === 'product' ? (
          <>
            <td>{item.price}</td>
            <td>{item.desc}</td>
          </>
        ) : (
          <>
            <td>{item.email}</td>
            <td>
              <span style={{ 
                padding: '4px 8px', 
                borderRadius: '4px', 
                backgroundColor: item.role === 'admin' ? '#ffe0e0' : '#e0ffe0',
                color: item.role === 'admin' ? '#d00' : '#080'
              }}>
                {item.role}
              </span>
            </td>
          </>
        )}
      </tr>
    ));
  };

  return (
    <div style={{minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <Header />
      
      <main style={{ paddingTop: '120px', paddingBottom: '60px', flexGrow: 1, backgroundColor: 'var(--color-scheme-1-foreground)'}}>
        <div className="admin-container">
          <h1 className="quality-title" style={{ marginBottom: '40px' }}>Admin Dashboard</h1>
          
          <div className="admin-header-controls">
            <select 
              className="admin-select" 
              value={entityType} 
              onChange={(e) => setEntityType(e.target.value)}
            >
              <option value="product">Products</option>
              <option value="user">Users</option>
            </select>

            <input 
              type="text" 
              className="admin-input" 
              placeholder={`Search ${entityType}s by name...`} 
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>

          <div className="admin-table-wrapper">
            <table className="admin-table">
              <thead>
                {renderTableHeaders()}
              </thead>
              <tbody>
                {renderTableRows()}
              </tbody>
            </table>
          </div>
        </div>
      </main>

    </div>
  );
};

export default Admin;
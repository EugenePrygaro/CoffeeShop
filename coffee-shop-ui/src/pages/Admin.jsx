import React, { useState, useEffect } from 'react';
import { Header } from '../components/Header';
import { richProductsWithImg } from '../js/products';

const Admin = () => {
  const [entityType, setEntityType] = useState('product');
  const [searchQuery, setSearchQuery] = useState('');
  const [operation, setOperation] = useState('create');
  const [formData, setFormData] = useState({});
  const [data, setData] = useState([]);

    const mockProducts = richProductsWithImg();

  const mockUsers = [
    { id: 1, name: 'Ivan Franko', email: 'ivan@example.com', role: 'admin' },
    { id: 2, name: 'Taras Shevchenko', email: 'taras@example.com', role: 'customer' },
    { id: 3, name: 'Lesya Ukrainka', email: 'lesya@example.com', role: 'customer' },
  ];
  const mockSubscriptions = [
    { id: 1, email: 'coffee-lover@gmail.com', date: '2025-02-10', status: 'active' },
    { id: 2, email: 'test-user@ukr.net', date: '2025-02-15', status: 'active' },
    { id: 3, email: 'old-subscriber@mail.com', date: '2024-12-01', status: 'inactive' },
  ];

  // Імітація запиту до бекенду при зміні типу сутності або пошуку
  useEffect(() => {
    // Тут у майбутньому буде: axios.get(`/api/${entityType}?search=${searchQuery}`)
    
    let currentData;
    if (entityType === 'product') currentData = mockProducts;
    else if (entityType === 'user') currentData = mockUsers;
    else currentData = mockSubscriptions;


    if (searchQuery.trim() !== '') {
      const query = searchQuery.toLowerCase();
      currentData = currentData.filter(item => 
        (item.name && item.name.toLowerCase().includes(query)) ||
        (item.email && item.email.toLowerCase().includes(query))
      );
    }
    setData(currentData);
  }, [entityType, searchQuery]);


  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleFormSubmit = (e) => {
    e.preventDefault();
    console.log(`Action: ${operation} on ${entityType}`, formData);
  };

  

 const renderTableHeaders = () => {
  if (entityType === 'product') {
    return (
      <tr>
        <th>ID</th>
        <th>Name</th>
        <th>Category</th>
        <th>Price</th>
        <th>Description</th>
      </tr>
    );
  } else if (entityType === 'user') {
    return (
      <tr>
        <th>ID</th>
        <th>Name</th>
        <th>Email</th>
        <th>Role</th>
      </tr>
    );
  } else {
    return (
      <tr>
        <th>ID</th>
        <th>Subscriber Email</th>
        <th>Subscription Date</th>
        <th>Status</th>
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
          <td>{item.category}</td>
          <td>{item.price} uah</td>
          <td>{item.desc}</td>
        </>
      )}

      {entityType === 'user' && (
        <>
          <td><strong>{item.name}</strong></td>
          <td>{item.email}</td>
          <td>
            <span style={{ 
              padding: '4px 8px', 
              borderRadius: '4px', 
              backgroundColor: item.role === 'admin' ? '#ffe0e0' : '#e0ffe0',
              color: item.role === 'admin' ? '#d00' : '#080',
              fontSize: '12px',
              fontWeight: '600'
            }}>
              {item.role}
            </span>
          </td>
        </>
      )}

      {entityType === 'subscription' && (
        <>
          <td><strong>{item.email}</strong></td>
          <td>{item.date}</td>
          <td>
            <span className={`status-badge ${item.status === 'active' ? 'status-active' : 'status-inactive'}`}>
              {item.status}
            </span>
          </td>
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
            <input name="name" placeholder="Name" onChange={handleInputChange} className="admin-input" />
            <input name="price" placeholder="Price" onChange={handleInputChange} className="admin-input" />
            <input name="desc" placeholder="Description" onChange={handleInputChange} className="admin-input" />
          </>
        )}
        {entityType === 'user' && (
          <>
            <input name="name" placeholder="Name" onChange={handleInputChange} className="admin-input" />
            <input name="email" placeholder="Email" onChange={handleInputChange} className="admin-input" />
          </>
        )}
        {entityType === 'subscription' && (
          <input name="email" placeholder="Subscriber Email" onChange={handleInputChange} className="admin-input" />
        )}
      </div>
    );
  };

  return (
    <div style={{minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <Header />
      
      <main style={{ paddingTop: '120px', paddingBottom: '60px', flexGrow: 1, backgroundColor: 'var(--color-scheme-1-foreground)'}}>
        <div className="admin-container">
          <h1 className="quality-title">Admin Dashboard</h1>
          
          <div className="admin-header-controls">
            <select 
              className="admin-select" 
              value={entityType} 
              onChange={(e) => setEntityType(e.target.value)}
            >
              <option value="product">Products</option>
              <option value="user">Users</option>
              <option value="subscription">Subscriptions</option>
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
        

        <section className="admin-management-section">
            <h2 className="quality-title-list">Manage data</h2>
            <div className="admin-form-controls">
              <select className="admin-select" value={operation} onChange={(e) => setOperation(e.target.value)}>
                <option value="create">Create</option>
                <option value="update">Update</option>
                <option value="delete">Delete</option>
              </select>
              
              {(operation === 'update' || operation === 'delete') && (
                <select className="admin-select" onChange={(e) => setFormData({id: e.target.value})}>
                  <option value="">Select ID to {operation}</option>
                  {data.map(item => <option key={item.id} value={item.id}>{item.name || item.email}</option>)}
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
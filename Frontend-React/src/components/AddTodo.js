import { Container, Row, Col, Button, Form, Stack } from 'react-bootstrap'
import React, { useState } from 'react'
import axios from 'axios'

export const AddTodo = ({ onAddSuccess }) => {
  const API_URL = `${process.env.REACT_APP_BASE_URL}/v1/api/todoitems`

  const [description, setDescription] = useState('')

  const handleDescriptionChange = (event) => {
    setDescription(event.target.value)
  }

  function handleClear() {
    setDescription('')
  }

  async function handleAdd() {
    axios
      .post(
        API_URL,
        { description: description, isCompleted: false },
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .then((response) => {
        //console.log(response)
        handleClear()
        onAddSuccess()
      })
      .catch((error) => {
        console.error(error)
        //TODO add toast or any other UI notification library
        if (error && error.response && error.response.status == 400) {
          if (typeof error.response.data === 'string') {
            alert('Error -' + error.response.data)
          } else {
            alert('Error -' + error.response.data.errors.Description[0])
          }
        }
      })
  }

  return (
    <Container>
      <h1>Add Item</h1>
      <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
        <Form.Label column sm="2">
          Description
        </Form.Label>
        <Col md="6">
          <Form.Control
            type="text"
            placeholder="Enter description..."
            value={description}
            onChange={handleDescriptionChange}
          />
        </Col>
      </Form.Group>
      <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
        <Stack direction="horizontal" gap={2}>
          <Button variant="primary" onClick={() => handleAdd()}>
            Add Item
          </Button>
          <Button variant="secondary" onClick={() => handleClear()}>
            Clear
          </Button>
        </Stack>
      </Form.Group>
    </Container>
  )
}

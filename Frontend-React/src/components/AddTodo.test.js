import { render, screen, waitFor, fireEvent } from '@testing-library/react'
import axios from 'axios'
import { AddTodo } from './AddTodo'

jest.mock('axios')

describe('AddTodo component', () => {
  beforeEach(() => {
    axios.post.mockReset()
  })

  it('loads successfully', () => {
    render(<AddTodo />)

    expect(screen.getByRole('button', { name: 'Add Item' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Clear' })).toBeInTheDocument()
  })

  it('clears description and handles description input correctly', () => {
    const todoDescription = 'Test desc'

    render(<AddTodo />)
    const todoClearButton = screen.getByRole('button', { name: 'Clear' })
    const todoDescriptionInput = screen.getByPlaceholderText('Enter description...')

    fireEvent.change(todoDescriptionInput, { target: { value: todoDescription } })

    expect(todoDescriptionInput).toHaveValue(todoDescription)

    fireEvent.click(todoClearButton)
    expect(todoDescriptionInput).toHaveValue('')
  })

  it('add a todo item', async () => {
    const onAddSuccess = jest.fn()
    render(<AddTodo onAddSuccess={onAddSuccess} />)

    const todoDescriptionInput = screen.getByPlaceholderText('Enter description...')
    const todoAddButton = screen.getByRole('button', { name: 'Add Item' })
    axios.post.mockResolvedValueOnce({ data: {} })
    console.log('todoDescriptionInput')

    fireEvent.change(todoDescriptionInput, { target: { value: 'Test Description' } })
    fireEvent.click(todoAddButton)

    await waitFor(() => {
      expect(onAddSuccess).toHaveBeenCalledTimes(1)
      expect(axios.post).toHaveBeenCalledTimes(1)
      expect(todoDescriptionInput).toHaveValue('')
    })
  })
})

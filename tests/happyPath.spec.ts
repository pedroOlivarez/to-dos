import { test, expect } from "@playwright/test";
const url = "http://localhost:5173";

const testUserEmail = `e2eTester${new Date().toISOString().split(".")[0].replace(/\D/g, "")}@gmail.biz`;

const giveItASecond = (delay = 250) =>
  new Promise((resolve) => setTimeout(resolve, delay));

test("happy path", async ({ page }) => {
  await test.step("register", async () => {
    await page.goto(`${url}/login`);

    const loginForm = page.getByTestId("login-form");
    expect(loginForm).toBeDefined();

    const emailInput = page.getByTestId("email-input");
    expect(emailInput).toBeVisible();

    await emailInput.fill(testUserEmail);

    const passwordInput = page.getByTestId("password-input");
    expect(passwordInput).toBeVisible();
    await passwordInput.fill("validpassword1");

    // validation triggered on blur
    await emailInput.focus();

    const registerButton = page.getByTestId("register-btn");
    expect(registerButton).toBeEnabled();
    await registerButton.click();
    await page.waitForURL(/\/to-dos/);
  });

  await test.step("add a to-do", async () => {
    await page.goto(`${url}/to-dos`);
    const logOutButton = page.getByTestId("log-out-btn");
    expect(logOutButton).toBeVisible();

    const addToDoButton = page.getByTestId("add-todo-btn");
    expect(addToDoButton).toBeVisible();

    await addToDoButton.click();

    const addToDoDialog = page.getByText("Create To-Do");
    expect(addToDoDialog).toBeVisible();

    const titleInput = page.getByTestId("title-input");
    await titleInput.fill("my to-do");

    const saveButton = page.getByTestId("create-todo-btn");
    expect(saveButton).toBeEnabled();

    await saveButton.click();

    await giveItASecond(500);
    expect(addToDoDialog).toBeHidden();

    const myToDo = page.getByText("my to-do");
    expect(myToDo).toBeVisible();
  });

  await test.step("edit a to-do", async () => {
    const myToDo = page.getByText("my to-do");
    expect(myToDo).toBeVisible();

    await myToDo.click();

    await giveItASecond(750);

    const editToDoDialog = page.getByText("Update To-Do");
    expect(editToDoDialog).toBeVisible();

    const titleInput = page.getByTestId("title-input");
    await titleInput.fill("OUR to-do");

    const bodyInput = page.getByTestId("body-input");
    await bodyInput.fill("some text");

    const saveButton = page.getByTestId("edit-todo-btn");
    expect(saveButton).toBeEnabled();

    await saveButton.click();

    await giveItASecond(500);
    expect(editToDoDialog).toBeHidden();

    const updatedToDo = page.getByText("OUR to-do");
    expect(updatedToDo).toBeVisible();
  });

  await test.step("archive a to-do", async () => {
    const ourToDo = page.getByText("OUR to-do");
    expect(ourToDo).toBeVisible();

    await ourToDo.click();

    await giveItASecond();

    const editToDoDialog = page.getByText("Update To-Do");
    expect(editToDoDialog).toBeVisible();

    const archiveButton = page.getByTestId("archive-todo-btn");
    expect(archiveButton).toBeEnabled();

    await archiveButton.click();

    await giveItASecond(500);
    expect(editToDoDialog).toBeHidden();

    const emptyResultText = page.getByText("Nothing to do so far...");
    expect(emptyResultText).toBeVisible();
  });

  await test.step("log out", async () => {
    const logOutButton = page.getByTestId("log-out-btn");
    expect(logOutButton).toBeVisible();

    await logOutButton.click();

    await page.waitForURL(/\/login/);
  });
});

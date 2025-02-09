/* eslint-disable no-undef */
describe("Authenticate and go to dashboard", () => {
    it("should log in", () => {
        cy.visit("http://localhost:3000/")
        cy.get("input[name=email]").type("basel.oms@email.test")
        cy.get("input[name=password]").type("Qwerty123!")
        cy.get("button[type=submit]").click()
        cy.url().should("include", "/dashboard")
    })
})


describe("Authenticate invalid credentials", () => {
    it("should not log in", () => {
        cy.visit("http://localhost:3000/")
        cy.get("input[name=email]").type("email@email.com")
        cy.get("input[name=password]").type("password")
        cy.get("button[type=submit]").click()
        cy.get("div[data-login-error]").should("contain", "Invalid email or password. Try again.")
    })
})
